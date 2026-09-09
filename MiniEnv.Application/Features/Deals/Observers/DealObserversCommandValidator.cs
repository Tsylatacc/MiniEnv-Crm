using FluentValidation;

namespace MiniEnv.Application.Features.Deals.Observers
{
    public sealed class DealObserversCommandValidator
        : AbstractValidator<DealObserversCommand>
    {
        public DealObserversCommandValidator()
        {
            RuleFor(x => x.DealId)
                .NotEqual(Guid.Empty);

            RuleForEach(x => x.ObserverIds)
                .NotEqual(Guid.Empty);
        }
    }
}