using FluentValidation;

namespace MiniEnv.Application.Features.Users.RevokeInvitation
{
    public class RevokeInvitationCommandValidator
        : AbstractValidator<RevokeInvitationCommand>
    {
        public RevokeInvitationCommandValidator()
        {
            RuleFor(x => x.InvitationId)
                .NotEmpty();
        }
    }
}
