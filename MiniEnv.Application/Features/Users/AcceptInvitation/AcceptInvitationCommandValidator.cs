using FluentValidation;

namespace MiniEnv.Application.Features.Users.AcceptInvitation
{
    public class AcceptInvitationCommandValidator
        : AbstractValidator<AcceptInvitationCommand>
    {
        public AcceptInvitationCommandValidator()
        {
            RuleFor(x => x.InvitationToken)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(64);
        }
    }
}
