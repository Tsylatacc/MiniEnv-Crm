using MiniEnv.Application.Features.Pipelines.Create;
using MiniEnv.Application.Features.Pipelines.Delete;
using MiniEnv.Application.Features.Pipelines.Kanban;
using MiniEnv.Application.Features.Pipelines.Patch;
using MiniEnv.Application.Features.Pipelines.ReorderStages;
using MiniEnv.Application.Features.Pipelines.StageDeals;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Authorization.Permissions;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Wolverine;
using Wolverine.Http;

namespace MiniEnv.Api.Controllers
{
    public static class PipelineEndpoints
    {
        [RequirePermission(Permissions.Names.ReadDeal)]
        [WolverineGet("/api/pipelines/{pipelineId:guid}/kanban")]
        public static async Task<KanbanResponse> Kanban(
            Guid pipelineId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<KanbanResponse>(
                currentUser.TenantId.ToString(),
                new KanbanCommand(pipelineId),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ReadDeal)]
        [WolverineGet("/api/pipelines/kanban/stages/{stageId}/deals")]
        public static async Task<KanbanResponse> StageDeals(
            Guid stageId,
            StageDealRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<KanbanResponse>(
                currentUser.TenantId.ToString(),
                new StageDealCommand(stageId, request.Skip),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ReadDeal)]
        [WolverineGet("/api/pipelines/{pipelineId:guid}/list")]
        public static async Task<KanbanResponse> List(
            Guid pipelineId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<KanbanResponse>(
                currentUser.TenantId.ToString(),
                new KanbanCommand(pipelineId),
                cancellationToken);
        }

        [WolverinePost("/api/pipelines")]
        public static async Task<CreatePipelineResponse> Create(
            CreatePipelineCommand command,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<CreatePipelineResponse>(
                currentUser.TenantId.ToString(),
                command,
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ManageConfigurations)]
        [WolverineDelete("/api/pipelines/{pipelineId:guid}")]
        public static async Task Delete(
            Guid pipelineId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync<DeletePipelineCommand>(
                currentUser.TenantId.ToString(),
                new DeletePipelineCommand(pipelineId),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ManageConfigurations)]
        [WolverinePatch("/api/pipelines/{pipelineId:guid}")]
        public static async Task Patch(
            Guid pipelineId,
            PatchPipelineRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync<PatchPipelineCommand>(
                currentUser.TenantId.ToString(),
                new PatchPipelineCommand
                (
                    pipelineId,
                    request.Name
                ),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ManageConfigurations)]
        [WolverinePut("/api/pipelines/{pipelineId:guid}/stages/reorder")]
        public static async Task ReorderStages(
            Guid pipelineId,
            ReorderStagesRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new ReorderStagesCommand(pipelineId, request.StageIds),
                cancellationToken);
        }
    }
}
