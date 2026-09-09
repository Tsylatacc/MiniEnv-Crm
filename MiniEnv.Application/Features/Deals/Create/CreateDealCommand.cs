namespace MiniEnv.Application.Features.Deals.Create
{
    public sealed record CreateDealCommand(
        Guid StageId,
        List<Guid>? CustomerIds);
}
