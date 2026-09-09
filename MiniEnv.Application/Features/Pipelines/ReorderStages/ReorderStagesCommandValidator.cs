using FluentValidation;

namespace MiniEnv.Application.Features.Pipelines.ReorderStages
{
    public class ReorderStagesCommandValidator
        : AbstractValidator<ReorderStagesCommand>
    {
        public ReorderStagesCommandValidator()
        {
            RuleFor(x => x.PipelineId)
                .NotEmpty();

            RuleForEach(x => x.StageIds)
                .NotEmpty();
        }
    }
}
