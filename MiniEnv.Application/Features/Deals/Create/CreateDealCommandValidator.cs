using FluentValidation;

namespace MiniEnv.Application.Features.Deals.Create
{
    public class CreateDealCommandValidator
        : AbstractValidator<CreateDealCommand>
    {
        public CreateDealCommandValidator()
        {
            RuleFor(x => x.StageId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleForEach(x => x.CustomerIds)
                .NotEqual(Guid.Empty)
                .When(x => x is not null);
        }
    }
}
