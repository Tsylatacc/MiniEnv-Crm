namespace MiniEnv.Application.Features.Authentication.ForgotPassword
{
    public sealed record PasswordResetRequested(
        Guid PasswordResetTokenId,
        string Email,
        string ForgotPasswordToken);
}
