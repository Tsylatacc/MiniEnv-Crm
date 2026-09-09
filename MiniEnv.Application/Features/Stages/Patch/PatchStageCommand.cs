namespace MiniEnv.Application.Features.Stages.Patch
{
    public sealed record PatchStageCommand(
        Guid StageId,
        string? Name,
        string? Color);

}
