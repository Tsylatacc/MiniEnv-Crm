namespace MiniEnv.Application.Features.Tenants.ActivateTenant
{
    public sealed record ActivateTenantResponse(
        string BearerToken,
        DateTimeOffset BearerTokenExpiresAt,
        Guid TenantId);
}
