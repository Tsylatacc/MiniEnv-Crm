using FluentValidation;

namespace MiniEnv.Application.Features.Pipelines.List
{
    public class ListCommandValidator
    : AbstractValidator<ListCommand>
    {
        public ListCommandValidator()
        {
            RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);
        }
    }
}
