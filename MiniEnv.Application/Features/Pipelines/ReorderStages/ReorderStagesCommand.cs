namespace MiniEnv.Application.Features.Pipelines.ReorderStages
{
    public sealed record ReorderStagesCommand(
        Guid PipelineId,
        List<Guid> StageIds);
}
