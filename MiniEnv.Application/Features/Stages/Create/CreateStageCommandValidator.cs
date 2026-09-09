using FluentValidation;

namespace MiniEnv.Application.Features.Stages.Create
{
    public class CreateStageCommandValidator
    : AbstractValidator<CreateStageCommand>
    {
        public CreateStageCommandValidator()
        {
            RuleFor(x => x.PipelineId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
