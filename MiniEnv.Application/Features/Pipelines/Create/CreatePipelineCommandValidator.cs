using FluentValidation;

namespace MiniEnv.Application.Features.Pipelines.Create
{
    public class CreatePipelineCommandValidator
        : AbstractValidator<CreatePipelineCommand>
    {
        public CreatePipelineCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
