using FluentValidation;

namespace MiniEnv.Application.Features.Authentication.Login
{
    public class LoginCommandValidator
        : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(64);
        }
    }
}
