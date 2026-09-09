using static MiniEnv.Application.Features.Deals.Get.GetDealHandler;

namespace MiniEnv.Application.Features.Deals.Get
{
    public sealed record GetDealResponse(
        DealDto? Deal);
}
