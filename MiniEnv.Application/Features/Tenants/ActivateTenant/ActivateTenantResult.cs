namespace MiniEnv.Application.Features.Tenants.ActivateTenant
{
    public sealed record ActivateTenantResult(
        string RefreshToken,
        string BearerToken,
        DateTimeOffset RefreshTokenExpiresAt,
        DateTimeOffset BearerTokenExpiresAt,
        Guid TenantId);
}
