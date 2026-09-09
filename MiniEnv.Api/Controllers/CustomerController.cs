using MiniEnv.Application.Features.Customers.Create;
using MiniEnv.Application.Features.Customers.Get;
using MiniEnv.Application.Features.Customers.GetDeals;
using MiniEnv.Application.Features.Customers.Patch;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Authorization.Permissions;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Wolverine;
using Wolverine.Http;

namespace MiniEnv.Api.Controllers
{
    public static class CustomerEndpoints
    {
        [WolverineGet("/api/customers/{customerId:guid}")]
        public static async Task<GetCustomerResponse> Get(
            Guid customerId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<GetCustomerResponse>(
                currentUser.TenantId.ToString(),
                new GetCustomerCommand(customerId),
                cancellationToken);
        }

        [WolverinePost("/api/customers")]
        public static async Task<CreateCustomerResponse> Create(
            CreateCustomerCommand command,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<CreateCustomerResponse>(
                currentUser.TenantId.ToString(),
                command,
                cancellationToken);
        }

        [WolverinePatch("/api/customers/{customerId:guid}")]
        public static async Task Patch(
            Guid customerId,
            PatchCustomerRequest request,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new PatchCustomerCommand(
                    customerId,
                    request.Name,
                    request.PhoneNumber,
                    request.Email
                    ),
                cancellationToken);
        }

        [RequirePermission(Permissions.Names.ReadDeal)]
        [WolverineGet("/api/customers/{customerId:guid}/deals")]
        public static async Task<GetCustomerDealsResponse> GetDeals(
            Guid customerId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            return await bus.InvokeForTenantAsync<GetCustomerDealsResponse>(
                currentUser.TenantId.ToString(),
                new GetCustomerDealsCommand(customerId),
                cancellationToken);
        }
    }
}
