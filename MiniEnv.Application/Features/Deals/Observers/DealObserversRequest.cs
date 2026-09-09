namespace MiniEnv.Application.Features.Deals.Observers
{
    public sealed record DealObserversRequest(
        IReadOnlyCollection<Guid> Observers);
}
