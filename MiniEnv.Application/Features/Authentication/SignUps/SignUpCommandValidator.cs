using FluentValidation;

namespace MiniEnv.Application.Features.Authentication.VerifySignUps
{
    public class SignUpCommandValidator
        : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
