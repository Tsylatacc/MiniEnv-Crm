using FluentValidation;

namespace MiniEnv.Application.Features.Pipelines.Patch
{
    public class PatchPipelineCommandValidator
        : AbstractValidator<PatchPipelineCommand>
    {
        public PatchPipelineCommandValidator()
        {
            RuleFor(x => x.PipelineId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Name)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.Name));
        }
    }
}
