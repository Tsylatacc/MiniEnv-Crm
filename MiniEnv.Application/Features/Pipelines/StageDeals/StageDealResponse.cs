using static MiniEnv.Application.Features.Pipelines.StageDeals.StageDealHandler;

namespace MiniEnv.Application.Features.Pipelines.StageDeals
{
    public sealed record StageDealResponse(
        IReadOnlyCollection<StageDealDto> Deals);
}
