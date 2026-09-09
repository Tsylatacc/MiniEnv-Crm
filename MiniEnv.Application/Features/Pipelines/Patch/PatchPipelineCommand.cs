namespace MiniEnv.Application.Features.Pipelines.Patch
{
    public sealed record PatchPipelineCommand(
        Guid PipelineId,
        string? Name
        );
}
