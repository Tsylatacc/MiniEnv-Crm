using FluentValidation;

namespace MiniEnv.Application.Features.Deals.Patch
{
    public class PatchDealCommandValidator
        : AbstractValidator<PatchDealCommand>
    {
        public PatchDealCommandValidator()
        {
            RuleFor(x => x.DealId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Title)
                .MaximumLength(255);
        }
    }
}
