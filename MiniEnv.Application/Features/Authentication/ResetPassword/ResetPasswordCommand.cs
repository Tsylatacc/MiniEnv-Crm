namespace MiniEnv.Application.Features.Authentication.ResetPassword
{
    public sealed record ResetPasswordCommand(
        string PasswordResetToken,
        string NewPassword);
}
