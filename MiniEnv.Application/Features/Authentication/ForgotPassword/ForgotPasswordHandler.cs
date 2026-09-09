using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Exceptions;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace MiniEnv.Application.Features.Authentication.ForgotPassword
{
    public class ForgotPasswordHandler
    {
        public static async Task Handle(
            ForgotPasswordCommand command,
            MiniEnvDbContext db,
            IJwtService jwtService,
            IMessageBus bus,
            ILogger<ForgotPasswordHandler> logger,
            CancellationToken cancellationToken)
        {
            ForgotPasswordDto? forgotPasswordDto = await db.UserCredentials
                .IgnoreQueryFilters()
                .Where(x => x.Email == command.Email)
                .Select(x => new ForgotPasswordDto
                {
                    UserId = x.UserId,
                    TenantId = Guid.Parse(x.User.TenantId)
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (forgotPasswordDto is null)
                return;

            PasswordResetToken? passwordResetToken = await db.PasswordResetTokens
                .Where(x => x.UserId == forgotPasswordDto.UserId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (passwordResetToken is not null &&
                passwordResetToken.UsedAt is null &&
                passwordResetToken.ExpiresAt > DateTimeOffset.UtcNow)
            {
                throw new ConflictException("A password reset token is already active.");
            }

            string forgotPasswordToken = jwtService.GenerateToken(4);
            string forgotPasswordTokenHash = jwtService.HashToken(forgotPasswordToken);

            PasswordResetToken newPasswordResetToken = PasswordResetToken.Create(
                forgotPasswordTokenHash,
                forgotPasswordDto.UserId,
                DateTimeOffset.UtcNow.AddMinutes(15));

            await db.PasswordResetTokens.AddAsync(newPasswordResetToken, cancellationToken);
            await bus.PublishAsync(new PasswordResetRequested(
                newPasswordResetToken.Id,
                command.Email,
                forgotPasswordToken));

            logger.LogInformation("Sent reset token to user {UserId}", forgotPasswordDto.UserId);
        }

        private sealed class ForgotPasswordDto
        {
            public Guid UserId { get; set; }
            public Guid TenantId { get; set; }
        }
    }
}
