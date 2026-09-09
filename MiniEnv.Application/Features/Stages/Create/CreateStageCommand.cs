namespace MiniEnv.Application.Features.Stages.Create
{
    public sealed record CreateStageCommand(
        Guid PipelineId,
        string Name
        );
}
