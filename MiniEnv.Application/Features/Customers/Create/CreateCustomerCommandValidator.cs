using FluentValidation;

namespace MiniEnv.Application.Features.Customers.Create
{
    public class CreateCustomerCommandValidator
        : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{7,14}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }
}
