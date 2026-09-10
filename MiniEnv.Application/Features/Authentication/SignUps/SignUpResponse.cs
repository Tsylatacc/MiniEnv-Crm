namespace MiniEnv.Application.Features.Authentication.VerifySignUps
{
    public sealed record SignUpResponse(
        string Token = default!,
        DateTimeOffset ExpiresAt = default!);
}
