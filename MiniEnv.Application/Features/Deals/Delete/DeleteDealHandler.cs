using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Persistence;

namespace MiniEnv.Application.Features.Deals.Delete
{
    public class DeleteDealHandler
    {
        public static async Task Handle(
            DeleteDealCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            Deal deal = await db.Deals.FindAsync(command.DealId, cancellationToken)
                ?? throw new KeyNotFoundException($"Deal {command.DealId} not found.");

            deal.Delete();
        }
    }
}
