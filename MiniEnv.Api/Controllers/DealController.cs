using MiniEnv.Application.Features.Deals.Create;
using MiniEnv.Application.Features.Deals.Delete;
using MiniEnv.Application.Features.Deals.Get;
using MiniEnv.Application.Features.Deals.Move;
using MiniEnv.Application.Features.Deals.Observers;
using MiniEnv.Application.Features.Deals.Patch;
using MiniEnv.Application.Features.Deals.Transfer;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Authorization.Permissions;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Wolverine;
using Wolverine.Http;

namespace MiniEnv.Api.Controllers
{
    public static class DealEndpoints
    {
        [WolverineGet("/api/deals/{dealId:guid}")]
        public static async Task<GetDealResponse> Get(
            Guid dealId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<GetDealResponse>(
                currentUser.TenantId.ToString(),
                new GetDealCommand(dealId),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.CreateDeal)]
        [WolverinePost("/api/deals")]
        public static async Task<CreateDealResponse> Create(
            CreateDealCommand command,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<CreateDealResponse>(
                currentUser.TenantId.ToString(),
                command,
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ManageConfigurations)]
        [WolverineDelete("/api/deals/{dealId:guid}")]
        public static async Task Delete(
            Guid dealId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new DeleteDealCommand(dealId),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.UpdateDeal)]
        [WolverinePatch("/api/deals/{dealId:guid}/move")]
        public static async Task Move(
            Guid dealId,
            MoveDealRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new MoveDealCommand(dealId, request.StageId),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.UpdateDeal)]
        [WolverinePut("/api/deals/{dealId:guid}/transfer")]
        public static async Task Transfer(
            Guid dealId,
            TransferDealRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new TransferDealCommand(dealId, request.UserId),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.UpdateDeal)]
        [WolverinePatch("/api/deals/{dealId:guid}")]
        public static async Task Patch(
            Guid dealId,
            PatchDealRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new PatchDealCommand(dealId, request.Title, request.Notes),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.UpdateDeal)]
        [WolverinePut("/api/deals/{dealId:guid}/observers")]
        public static async Task Observers(
            Guid dealId,
            DealObserversRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new DealObserversCommand(dealId, request.Observers),
                cancellationToken);
        }

    }
}
