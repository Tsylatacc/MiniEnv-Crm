using FluentValidation;

namespace MiniEnv.Application.Features.Customers.Get
{
    public class GetCustomerCommandValidator
        : AbstractValidator<GetCustomerCommand>
    {
        public GetCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
