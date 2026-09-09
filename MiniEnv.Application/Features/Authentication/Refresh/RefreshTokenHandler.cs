using MiniEnv.Application.Features.Authentication.Refresh;
using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.DTOs.Common.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class RefreshHandler
{
    public static async Task<RefreshTokenResult> Handle(
        RefreshTokenCommand command,
        MiniEnvDbContext db,
        IJwtService jwtService,
        ILogger<RefreshHandler> logger,
        CancellationToken cancellationToken)
    {
        string refreshTokenHash = jwtService.HashToken(command.RefreshToken);

        RefreshTokenDto refreshTokenDto =
            await db.RefreshTokens
                .IgnoreQueryFilters()
                .Where(x => x.TokenHash == refreshTokenHash)
                .Select(x => new RefreshTokenDto
                {
                    RefreshToken = x,
                    UserId = x.UserId,
                    TenantId = Guid.Parse(x.User.TenantId),
                    Email = x.User.Credential != null
                        ? x.User.Credential.Email
                        : null
                })
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        refreshTokenDto.RefreshToken.Validate();

        if (string.IsNullOrWhiteSpace(refreshTokenDto.Email))
            throw new InvalidOperationException("User does not have a valid email.");

        JwtDto bearerDto = jwtService.GenerateBearerToken(
            refreshTokenDto.UserId,
            refreshTokenDto.TenantId,
            refreshTokenDto.Email);

        JwtDto tokenDto = jwtService.GenerateRefreshToken();

        string newRefreshTokenHash = jwtService.HashToken(tokenDto.Token);

        RefreshToken newRefreshTokenEntity = RefreshToken.Create(
                newRefreshTokenHash,
                refreshTokenDto.UserId,
                tokenDto.ExpiresAt);

        refreshTokenDto.RefreshToken.Revoke(newRefreshTokenEntity.Id);

        await db.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);
        logger.LogInformation("Refresh token rotated for user {UserId}", refreshTokenDto.UserId);

        return new RefreshTokenResult(
            tokenDto.Token,
            bearerDto.Token,
            newRefreshTokenEntity.ExpiresAt,
            bearerDto.ExpiresAt,
            refreshTokenDto.TenantId);
    }

    private sealed class RefreshTokenDto
    {
        public RefreshToken RefreshToken { get; init; } = default!;
        public Guid UserId { get; init; }
        public Guid TenantId { get; init; }
        public string? Email { get; init; }
    }
}