using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.DTOs.Common.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniEnv.Application.Features.Authentication.Login
{
    public class LoginHandler
    {
        public static async Task<LoginResult> Handle(
            LoginCommand command,
            MiniEnvDbContext db,
            IJwtService jwtService,
            ILogger<LoginHandler> logger,
            CancellationToken cancellationToken)
        {
            CredentialDto credentialDto = await db.UserCredentials
                    .IgnoreQueryFilters()
                    .Where(x => x.Email == command.Email &&
                           x.User.Status == UserStatus.Active)
                    .Select(x => new CredentialDto
                    {
                        UserId = x.UserId,
                        TenantId = Guid.Parse(x.User.TenantId),
                    })
                    .SingleOrDefaultAsync(cancellationToken)
                ?? throw new UnauthorizedAccessException("Invalid credentials.");

            JwtDto bearerDto = jwtService.GenerateBearerToken(
                credentialDto.UserId,
                credentialDto.TenantId,
                command.Email);

            JwtDto tokenDto = jwtService.GenerateRefreshToken();

            string newRefreshTokenHash = jwtService.HashToken(tokenDto.Token);

            RefreshToken newRefreshTokenEntity = RefreshToken.Create(
                    newRefreshTokenHash,
                    credentialDto.UserId,
                    tokenDto.ExpiresAt);

            await db.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);
            logger.LogInformation("User {UserId} has logged in", credentialDto.UserId);

            return new LoginResult(
                tokenDto.Token,
                bearerDto.Token,
                newRefreshTokenEntity.ExpiresAt,
                bearerDto.ExpiresAt,
                credentialDto.TenantId);
        }

        private sealed class CredentialDto
        {
            public Guid UserId { get; init; }
            public Guid TenantId { get; init; }
        }
    }
}
