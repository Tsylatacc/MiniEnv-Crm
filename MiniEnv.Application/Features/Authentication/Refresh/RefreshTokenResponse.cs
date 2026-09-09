namespace MiniEnv.Application.Features.Authentication.Refresh
{
    public sealed record RefreshTokenResponse(
        string BearerToken,
        DateTimeOffset BearerTokenExpiresAt,
        Guid TenantId);
}
