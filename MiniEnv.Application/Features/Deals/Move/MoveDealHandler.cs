using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Exceptions;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Deals.Move
{
    public class MoveDealHandler
    {
        public static async Task Handle(
            MoveDealCommand command,
            MiniEnvDbContext db,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
            if (!await db.Stages.AnyAsync(x => x.Id == command.StageId, cancellationToken))
                throw new KeyNotFoundException($"Stage {command.StageId} not found.");

            Deal deal = await db.Deals.FindAsync(command.DealId, cancellationToken) ??
               throw new KeyNotFoundException($"Deal {command.DealId} not found.");

            await currentUser.LoadPermissions(cancellationToken);
            AccessLevel accessLevel = currentUser.Permissions.PermissionsAccessLevel[Permissions.Names.UpdateDeal];

            switch (accessLevel)
            {
                case AccessLevel.All:
                    break;

                case AccessLevel.Own:
                    if (deal.OwnerId != currentUser.UserId)
                        throw new ForbiddenException();

                    break;

                default:
                    throw new ForbiddenException();
            }

            deal.Move(command.StageId);
        }
    }
}
