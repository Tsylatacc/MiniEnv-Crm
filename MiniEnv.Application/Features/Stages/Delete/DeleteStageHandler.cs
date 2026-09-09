using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Stages.Delete
{
    public class DeleteStageHandler
    {
        public static async Task Handle(
            DeleteStageCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Deal> deals = await db.Stages
                .Where(x => x.Id == command.StageId)
                .Select(x => x.Deals)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException($"Stage {command.StageId} not found.");

            if (deals.Count != 0)
                throw new InvalidOperationException($"Stage {command.StageId} cannot be deleted.");

            await db.Stages
                .Where(x => x.Id == command.StageId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
