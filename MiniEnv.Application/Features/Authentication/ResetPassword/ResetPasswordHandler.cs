using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniEnv.Application.Features.Authentication.ResetPassword
{
    public class ResetPasswordHandler
    {
        public static async Task Handle(
            ResetPasswordCommand command,
            MiniEnvDbContext db,
            IJwtService jwtService,
            ILogger<ResetPasswordHandler> logger,
            CancellationToken cancellationToken)
        {
            string resetTokenHash = jwtService.HashToken(command.PasswordResetToken);
            PasswordResetTokenDto passwordResetTokenDto = await db.PasswordResetTokens
                .IgnoreQueryFilters()
                .Where(x => x.TokenHash == resetTokenHash &&
                        x.UsedAt == null &&
                        x.ExpiresAt > DateTimeOffset.UtcNow)
                .Select(x => new PasswordResetTokenDto
                {
                    PasswordResetToken = x,
                    Credential = x.User.Credential,
                    RefreshTokens = x.User.RefreshTokens
                })
                .SingleOrDefaultAsync(cancellationToken) ??
                throw new KeyNotFoundException($"Password reset token {command.PasswordResetToken} not found.");

            if (passwordResetTokenDto.Credential is null)
                throw new KeyNotFoundException($"User credential not found for token {command.PasswordResetToken}.");

            string passwordHash = jwtService.HashPassword(command.NewPassword);
            passwordResetTokenDto.Credential.SetPassword(passwordHash);

            passwordResetTokenDto.PasswordResetToken.Consume();

            if (passwordResetTokenDto.RefreshTokens?.Count > 0)
            {
                foreach (RefreshToken refreshToken in passwordResetTokenDto.RefreshTokens)
                {
                    refreshToken.Revoke(null);
                }
            }
            logger.LogInformation("Password has been reset for user {UserId}", passwordResetTokenDto.Credential.UserId);
        }

        private sealed class PasswordResetTokenDto
        {
            public PasswordResetToken PasswordResetToken { get; init; } = default!;
            public UserCredential? Credential { get; init; } = default!;
            public IReadOnlyCollection<RefreshToken>? RefreshTokens { get; init; } = default!;
        }
    }
}
