using FluentValidation;

namespace MiniEnv.Application.Features.Deals.Customers
{
    public sealed class DealCustomersCommandValidator
        : AbstractValidator<DealCustomersCommand>
    {
        public DealCustomersCommandValidator()
        {
            RuleFor(x => x.DealId)
                .NotEqual(Guid.Empty);

            RuleForEach(x => x.CustomerIds)
                .NotEqual(Guid.Empty);
        }
    }
}