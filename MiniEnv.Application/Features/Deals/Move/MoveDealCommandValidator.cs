using FluentValidation;

namespace MiniEnv.Application.Features.Deals.Move
{
    public class MoveDealCommandValidator
        : AbstractValidator<MoveDealCommand>
    {
        public MoveDealCommandValidator()
        {
            RuleFor(x => x.DealId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.StageId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
