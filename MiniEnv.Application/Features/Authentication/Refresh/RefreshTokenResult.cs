namespace MiniEnv.Application.Features.Authentication.Refresh
{
    public sealed record RefreshTokenResult(
        string RefreshToken,
        string BearerToken,
        DateTimeOffset RefreshTokenExpiresAt,
        DateTimeOffset BearerTokenExpiresAt,
        Guid TenantId);
}
