using FluentValidation;

namespace MiniEnv.Application.Features.Pipelines.Kanban
{
    public class KanbanCommandValidator
    : AbstractValidator<KanbanCommand>
    {
        public KanbanCommandValidator()
        {
            RuleFor(x => x.PipelineId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
