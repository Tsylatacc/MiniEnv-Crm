using FluentValidation;

namespace MiniEnv.Application.Features.Pipelines.StageDeals
{
    public class StageDealCommandValidator
    : AbstractValidator<StageDealCommand>
    {
        public StageDealCommandValidator()
        {
            RuleFor(x => x.Skip)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.StageId)
                .NotEmpty();
        }
    }
}
