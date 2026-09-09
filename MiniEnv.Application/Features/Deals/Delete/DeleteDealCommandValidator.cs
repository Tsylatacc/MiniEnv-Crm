using FluentValidation;

namespace MiniEnv.Application.Features.Deals.Delete
{
    public class DeleteDealCommandValidator
        : AbstractValidator<DeleteDealCommand>
    {
        public DeleteDealCommandValidator()
        {
            RuleFor(x => x.DealId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
