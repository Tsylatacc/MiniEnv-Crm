using FluentValidation;

namespace MiniEnv.Application.Features.Authentication.ResetPassword
{
    public class ResetPasswordCommandValidator
        : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.PasswordResetToken)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(@"^[\x20-\x7E]+$")
                .MinimumLength(12)
                .MaximumLength(64);
        }
    }
}

