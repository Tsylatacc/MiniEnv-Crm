using FluentValidation;

namespace MiniEnv.Application.Features.Deals.Get
{
    public class GetDealCommandValidator
        : AbstractValidator<GetDealCommand>
    {
        public GetDealCommandValidator()
        {
            RuleFor(x => x.DealId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
