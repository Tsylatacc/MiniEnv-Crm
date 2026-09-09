using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Exceptions;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wolverine;

namespace MiniEnv.Application.Features.Users.Invite
{
    public class InviteHandler
    {
        public static async Task Handle(
            InviteCommand command,
            MiniEnvDbContext db,
            IJwtService jwtService,
            IMessageBus bus,
            ILogger<InviteHandler> logger,
            CancellationToken cancellationToken)
        {
            if (command.RoleId.HasValue &&
                !await db.Roles.AnyAsync(
                    x => x.Id == command.RoleId.Value,
                    cancellationToken))
            {
                throw new KeyNotFoundException($"Role {command.RoleId} not found.");
            }

            UserCredential? credential = await db.UserCredentials
                .Include(x => x.User)
                    .ThenInclude(x => x.Invitations)
                .SingleOrDefaultAsync(x => x.Email == command.Email, cancellationToken);

            User user;
            if (credential is not null)
            {
                user = credential.User;

                if (user.Status != UserStatus.PendingActivation)
                    throw new ConflictException($"Email {command.Email} is already registered.");

                if (command.RoleId.HasValue)
                    user.SetRole(command.RoleId.Value);
            }
            else
            {
                user = User.Create(
                    command.Name,
                    UserStatus.PendingActivation,
                    EntityOrigin.Custom);

                if (command.RoleId.HasValue)
                    user.SetRole(command.RoleId.Value);

                credential = UserCredential.Create(
                    command.Email,
                    user.Id);

                user.SetCredential(credential);

                await db.Users.AddAsync(user, cancellationToken);
            }

            Invitation? activeInvitation = user.GetActiveInvitation();
            if (activeInvitation is not null)
                activeInvitation.Revoke();

            string invitationToken = jwtService.GenerateToken(4);
            string invitationTokenHash = jwtService.HashToken(invitationToken);

            Invitation invitation = Invitation.Create(
                invitationTokenHash,
                user.Id,
                DateTimeOffset.UtcNow.AddMinutes(60));

            await db.Invitations.AddAsync(invitation, cancellationToken);

            await bus.PublishAsync(
                new InvitationRequested(
                    invitation.Id,
                    command.Email,
                    invitationToken));

            logger.LogInformation("Invitation {InvitationId} created for user {UserId}", invitation.Id, user.Id);
        }
    }
}