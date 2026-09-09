using FluentValidation;

namespace MiniEnv.Application.Features.Users.Invite
{
    public class InviteCommandValidator
        : AbstractValidator<InviteCommand>
    {
        public InviteCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.RoleId)
                .NotEmpty()
                .When(x => x.RoleId is not null);
        }
    }
}
