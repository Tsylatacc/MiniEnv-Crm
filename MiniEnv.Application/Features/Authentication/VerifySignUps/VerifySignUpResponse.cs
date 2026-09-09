namespace MiniEnv.Application.Features.Authentication.VerifySignUps
{
    public sealed record VerifySignUpResponse(
        string Token = default!,
        DateTimeOffset ExpiresAt = default!);
}
