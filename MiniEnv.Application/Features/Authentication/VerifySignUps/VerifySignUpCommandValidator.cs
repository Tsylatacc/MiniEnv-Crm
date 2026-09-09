using FluentValidation;

namespace MiniEnv.Application.Features.Authentication.VerifySignUps
{
    public class VerifySignUpCommandValidator
        : AbstractValidator<VerifySignUpCommand>
    {
        public VerifySignUpCommandValidator()
        {
            RuleFor(x => x.SignUpToken)
                .NotEmpty();
        }
    }
}
