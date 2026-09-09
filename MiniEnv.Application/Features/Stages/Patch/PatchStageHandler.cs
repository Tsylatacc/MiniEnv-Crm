using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Stages.Patch
{
    public class PatchStageHandler
    {
        public static async Task Handle(
            PatchStageCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            await db.Stages
                .Where(x => x.Id == command.StageId
                    && x.Outcome != StageOutcome.Won
                    && x.Outcome != StageOutcome.Lost)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Name, x => command.Name ?? x.Name)
                    .SetProperty(x => x.Color, x => command.Color ?? x.Color)
                    .SetProperty(x => x.UpdatedAt, _ => DateTimeOffset.UtcNow),
                    cancellationToken);
        }
    }
}
