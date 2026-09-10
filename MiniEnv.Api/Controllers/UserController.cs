using MiniEnv.Application.Features.Users.AcceptInvitation;
using MiniEnv.Application.Features.Users.Invite;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Authorization.Permissions;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        [RequirePermission(Permissions.Names.ManageUsers)]
        [WolverinePut("/api/users/invitations/revoke/{invitationId:guid}")]
        public static async Task Revoke(
            Guid invitationId,
            ICurrentUser currentUser,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
