namespace MiniEnv.Application.Features.Authentication.Login
{
    public sealed record LoginResponse(
        string BearerToken,
        DateTimeOffset BearerTokenExpiresAt,
        Guid TenantId);
}
