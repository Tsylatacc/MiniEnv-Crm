using MiniEnv.Application.Features.Stages.Create;
using MiniEnv.Application.Features.Stages.Patch;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Authorization.Permissions;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Wolverine;
using Wolverine.Http;

namespace MiniEnv.Api.Controllers
{
    public static class StageEndpoints
    {
        [RequirePermission(Permissions.Names.ManageConfigurations)]
        [WolverinePost("/api/stages")]
        public static async Task<CreateStageResponse> Create(
            CreateStageCommand command,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<CreateStageResponse>(
                currentUser.TenantId.ToString(),
                command,
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ManageConfigurations)]
        [WolverineDelete("/api/stages/{stageId:guid}")]
        public static async Task<CreateStageResponse> Delete(
            CreateStageCommand command,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<CreateStageResponse>(
                currentUser.TenantId.ToString(),
                command,
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ManageConfigurations)]
        [WolverinePatch("/api/stages/{stageId:guid}")]
        public static async Task Patch(
            Guid stageId,
            PatchStageRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new PatchStageCommand
                (
                    stageId,
                    request.Name,
                    request.Color
                ),
                cancellationToken);
        }
    }
}
