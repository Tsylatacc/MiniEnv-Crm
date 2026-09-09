namespace MiniEnv.Application.Features.Authentication.Login
{
    public sealed record LoginResult(
        string RefreshToken,
        string BearerToken,
        DateTimeOffset RefreshTokenExpiresAt,
        DateTimeOffset BearerTokenExpiresAt,
        Guid TenantId);
}
