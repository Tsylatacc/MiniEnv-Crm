namespace MiniEnv.Application.Features.Deals.Move
{
    public sealed record MoveDealCommand(
        Guid DealId,
        Guid StageId);
}
