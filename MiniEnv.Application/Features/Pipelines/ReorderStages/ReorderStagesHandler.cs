using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Pipelines.ReorderStages
{
    public class ReorderStagesHandler
    {
        public static async Task Handle(
            ReorderStagesCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            Pipeline pipeline = await db.Pipelines.FindAsync(command.PipelineId, cancellationToken)
                ?? throw new KeyNotFoundException($"Pipeline {command.PipelineId} not found.");

            List<Guid> stageIds = command.StageIds
                .Distinct()
                .ToList();

            if (await db.Stages.Where(x =>
                    x.PipelineId == command.PipelineId &&
                    stageIds.Contains(x.Id))
                .CountAsync(cancellationToken) != stageIds.Count)
                throw new InvalidOperationException("One or more stages were not found or do not belong to the pipeline.");

            pipeline.ReorderStages(stageIds);
        }
    }
}
