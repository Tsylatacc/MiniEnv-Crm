using FluentValidation;

namespace MiniEnv.Application.Features.Deals.Transfer
{
    public class TransferDealCommandValidator
        : AbstractValidator<TransferDealCommand>
    {
        public TransferDealCommandValidator()
        {
            RuleFor(x => x.DealId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.UserId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

        }
    }
}
