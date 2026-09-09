using FluentValidation;

namespace MiniEnv.Application.Features.Stages.Delete
{
    public class DeleteStageCommandValidator
        : AbstractValidator<DeleteStageCommand>
    {
        public DeleteStageCommandValidator()
        {
            RuleFor(x => x.StageId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
