using static MiniEnv.Application.Features.Pipelines.List.ListHandler;

namespace MiniEnv.Application.Features.Pipelines.List
{
    public sealed record ListResponse(
        IReadOnlyCollection<ListDealDto>? Deals,
        int TotalCount);
}
