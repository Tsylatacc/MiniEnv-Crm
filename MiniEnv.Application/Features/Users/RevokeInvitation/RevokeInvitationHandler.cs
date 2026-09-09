using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace MiniEnv.Application.Features.Users.RevokeInvitation
{
    public class RevokeInvitationHandler
    {
        public static async Task Handle(
            RevokeInvitationCommand command,
            MiniEnvDbContext db,
            ILogger<RevokeInvitationHandler> logger,
            CancellationToken cancellationToken)
        {
            Invitation invitation = await db.Invitations.FindAsync(command.InvitationId, cancellationToken)
                ?? throw new KeyNotFoundException($"Invitation {command.InvitationId} not found.");

            invitation.Revoke();
            logger.LogInformation("Invitation {InvitationId} revoked", invitation.Id);
        }
    }
}
