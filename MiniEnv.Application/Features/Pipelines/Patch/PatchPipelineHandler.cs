using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Pipelines.Patch
{
    public class PatchPipelineHandler
    {
        public static async Task Handle(
            PatchPipelineCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            await db.Pipelines
               .Where(x => x.Id == command.PipelineId)
               .ExecuteUpdateAsync(setters => setters
                   .SetProperty(x => x.Name, x => command.Name ?? x.Name)
                   .SetProperty(x => x.UpdatedAt, _ => DateTimeOffset.UtcNow),
                   cancellationToken);
        }
    }
}
