using FluentValidation;

namespace MiniEnv.Application.Features.Customers.Patch
{
    public class PatchCustomerCommandValidator
        : AbstractValidator<PatchCustomerCommand>
    {
        public PatchCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Name)
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
