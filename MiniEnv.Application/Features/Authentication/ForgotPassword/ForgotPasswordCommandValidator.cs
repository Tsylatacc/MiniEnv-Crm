using FluentValidation;

namespace MiniEnv.Application.Features.Authentication.ForgotPassword
{
    public class ForgotPasswordCommandValidator
        : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();
        }
    }
}
