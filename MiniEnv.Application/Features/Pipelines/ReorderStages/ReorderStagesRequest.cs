namespace MiniEnv.Application.Features.Pipelines.ReorderStages
{
    public sealed record ReorderStagesRequest(
        List<Guid> StageIds);
}
