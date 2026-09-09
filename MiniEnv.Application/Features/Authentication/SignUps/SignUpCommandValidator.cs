using FluentValidation;

namespace MiniEnv.Application.Features.Authentication.SignUps
{
    public class SignUpCommandValidator
        : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);
        }
    }
}
