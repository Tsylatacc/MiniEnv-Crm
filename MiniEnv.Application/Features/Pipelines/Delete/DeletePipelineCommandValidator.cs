using FluentValidation;

namespace MiniEnv.Application.Features.Pipelines.Delete
{
    public class DeletePipelineCommandValidator
        : AbstractValidator<DeletePipelineCommand>
    {
        public DeletePipelineCommandValidator()
        {
            RuleFor(x => x.PipelineId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
