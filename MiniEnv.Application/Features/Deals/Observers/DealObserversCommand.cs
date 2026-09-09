namespace MiniEnv.Application.Features.Deals.Observers
{
    public sealed record DealObserversCommand(
        Guid DealId,
        IReadOnlyCollection<Guid> ObserverIds);
}
