using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Deals.Patch
{
    public class PatchDealHandler
    {
        public static async Task Handle(
            PatchDealCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            await db.Deals
               .Where(x => x.Id == command.DealId)
               .ExecuteUpdateAsync(setters => setters
                   .SetProperty(x => x.Title, x => command.Title ?? x.Title)
                   .SetProperty(x => x.Notes, x => command.Notes ?? x.Notes)
                   .SetProperty(x => x.UpdatedAt, _ => DateTimeOffset.UtcNow),
                   cancellationToken);
        }
    }
}
