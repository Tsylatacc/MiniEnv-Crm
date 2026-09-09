using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Pipelines.Delete
{
    public class DeletePipelineHandler
    {
        public static async Task Handle(
            DeletePipelineCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Deal> deals = await db.Stages
                .Where(x => x.PipelineId == command.PipelineId)
                .Select(x => x.Deals)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException($"Pipeline {command.PipelineId} not found.");

            if (deals.Count != 0)
                throw new InvalidOperationException($"Pipeline {command.PipelineId} cannot be deleted.");

            await db.Pipelines
                .Where(x => x.Id == command.PipelineId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
