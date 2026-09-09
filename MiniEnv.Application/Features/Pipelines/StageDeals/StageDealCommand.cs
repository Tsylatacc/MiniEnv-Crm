namespace MiniEnv.Application.Features.Pipelines.StageDeals
{
    public sealed record StageDealCommand(
        Guid StageId,
        int Skip);
}
