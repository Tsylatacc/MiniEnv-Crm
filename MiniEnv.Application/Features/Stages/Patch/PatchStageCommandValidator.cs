using FluentValidation;

namespace MiniEnv.Application.Features.Stages.Patch
{
    public class PatchStageCommandValidator
        : AbstractValidator<PatchStageCommand>
    {
        public PatchStageCommandValidator()
        {
            RuleFor(x => x.StageId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Color)
                .Matches("^#[0-9A-Fa-f]{6}$")
                .When(x => !string.IsNullOrWhiteSpace(x.Color));
        }
    }
}
