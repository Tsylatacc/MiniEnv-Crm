namespace MiniEnv.Application.Features.Deals.Patch
{
    public sealed record PatchDealCommand(
        Guid DealId,
        string? Title,
        string? Notes);
}
