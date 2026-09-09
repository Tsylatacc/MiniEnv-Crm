using FluentValidation;

namespace MiniEnv.Application.Features.Authentication.Refresh
{
    public class RefreshTokenCommandValidator
        : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }
}
