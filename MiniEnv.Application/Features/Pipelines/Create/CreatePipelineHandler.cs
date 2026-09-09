using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Persistence;

namespace MiniEnv.Application.Features.Pipelines.Create
{
    public class CreatePipelineHandler
    {
        public static async Task<CreatePipelineResponse> Handle(
            CreatePipelineCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            Pipeline pipeline = Pipeline.Create(command.Name, EntityOrigin.Custom);
            await db.AddAsync(pipeline, cancellationToken);
            return new CreatePipelineResponse(pipeline.Id);
        }
    }
}
