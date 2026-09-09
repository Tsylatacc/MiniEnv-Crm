using static MiniEnv.Application.Features.Customers.GetDeals.GetCustomerDealsHandler;

namespace MiniEnv.Application.Features.Customers.GetDeals
{
    public sealed record GetCustomerDealsResponse(
        CustomerDealsDto? CustomerDeals);
}
