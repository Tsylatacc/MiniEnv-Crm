using MiniEnv.Application.Features.Users.AcceptInvitation;
using MiniEnv.Application.Features.Users.Invite;
using MiniEnv.Application.Features.Users.RevokeInvitation;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Authorization.Permissions;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.Http;

namespace MiniEnv.Api.Controllers
{
    public static class UserEndpoints
    {
        [RequirePermission(Permissions.Names.ManageUsers)]
        [WolverinePost("/api/users/invitations/invite")]
        public static async Task Invite(
            InviteCommand command,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                command,
                cancellationToken);
        }

        [AllowAnonymous]
        [WolverinePut("/api/users/invitations/accept")]
        public static async Task Accept(
            AcceptInvitationCommand command,
            IMessageBus bus,
            IJwtService jwtService,
            MiniEnvDbContext lookupDb,
            CancellationToken cancellationToken)
        {
            string invitationTokenHash = jwtService.HashToken(command.InvitationToken);

            string? tenantId = await lookupDb.Invitations
                .IgnoreQueryFilters()
                .Where(x => x.TokenHash == invitationTokenHash)
                .Select(x => x.TenantId)
                .SingleOrDefaultAsync(cancellationToken);

            if (tenantId is null) return;

            await bus.InvokeForTenantAsync(tenantId, command, cancellationToken);
        }

        [RequirePermission(Permissions.Names.ManageUsers)]
        [WolverinePut("/api/users/invitations/revoke/{invitationId:guid}")]
        public static async Task Revoke(
            Guid invitationId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            await bus.InvokeForTenantAsync(
                currentUser.TenantId.ToString(),
                new RevokeInvitationCommand(invitationId),
                cancellationToken);
        }
    }
}
