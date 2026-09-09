using MiniEnv.Domain.Entities;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MiniEnv.Application.Features.Users.AcceptInvitation
{
    public class AcceptInvitationHandler
    {
        public static async Task Handle(
            AcceptInvitationCommand command,
            MiniEnvDbContext db,
            IJwtService jwtService,
            ILogger<AcceptInvitationHandler> logger,
            CancellationToken cancellationToken)
        {
            string invitationTokenHash = jwtService.HashToken(command.InvitationToken);
            Invitation? invitation = await db.Invitations
                .Where(x => x.TokenHash == invitationTokenHash)
                .Include(x => x.User)
                .SingleOrDefaultAsync(cancellationToken);

            if (invitation is null) return;

            invitation.Accept();
            logger.LogInformation("Invitation {InvitationId} accepted by user {UserId}", invitation.Id, invitation.UserId);
        }
    }
}
