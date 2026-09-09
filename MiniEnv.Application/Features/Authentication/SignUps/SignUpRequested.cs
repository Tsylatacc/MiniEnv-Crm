namespace MiniEnv.Application.Features.Authentication.SignUps
{
    public sealed record SignUpRequested(
        Guid SignUpId,
        string Email,
        string SignUpToken);
}
