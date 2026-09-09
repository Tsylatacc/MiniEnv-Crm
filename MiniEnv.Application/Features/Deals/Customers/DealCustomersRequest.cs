namespace MiniEnv.Application.Features.Deals.Customers
{
    public sealed record DealCustomersRequest(
        IReadOnlyCollection<Guid> Customers);
}
