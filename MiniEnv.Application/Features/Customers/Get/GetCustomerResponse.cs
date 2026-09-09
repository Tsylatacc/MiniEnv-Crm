using static MiniEnv.Application.Features.Customers.Get.GetCustomerHandler;

namespace MiniEnv.Application.Features.Customers.Get
{
    public sealed record GetCustomerResponse(
        CustomerDto? Customer);
}
