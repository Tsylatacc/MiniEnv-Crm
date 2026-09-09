namespace MiniEnv.Application.Features.Deals.Customers
{
    public sealed record DealCustomersCommand(
        Guid DealId,
        IReadOnlyCollection<Guid> CustomerIds);
}
