using MiniEnv.Application.Features.Customers.Get;
using FluentValidation;

namespace MiniEnv.Application.Features.Customers.GetDeals
{
    public class GetDealsCommandValidator
        : AbstractValidator<GetCustomerCommand>
    {
        public GetDealsCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
