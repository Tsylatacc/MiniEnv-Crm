namespace MiniEnv.Application.Features.Deals.Transfer
{
    public sealed record TransferDealCommand(
        Guid DealId,
        Guid UserId);
}
