using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Persistence;

namespace MiniEnv.Application.Features.Stages.Create
{
    public class CreateStageHandler
    {
        public static async Task<CreateStageResponse> Handle(
            CreateStageCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            Pipeline pipeline = await db.Pipelines.FindAsync(command.PipelineId, cancellationToken)
                ?? throw new KeyNotFoundException($"Pipeline {command.PipelineId} not found.");

            Stage stage = pipeline.AddStage(command.Name, EntityOrigin.Custom);
            return new CreateStageResponse(stage.Id);
        }
    }
}
