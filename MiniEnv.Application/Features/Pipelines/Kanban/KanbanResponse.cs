using static MiniEnv.Application.Features.Pipelines.Kanban.KanbanHandler;

namespace MiniEnv.Application.Features.Pipelines.Kanban
{
    public sealed record KanbanResponse(
        KanbanDto? Pipeline
        );
}
