using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Exceptions;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Deals.Observers
{
    public class DealObserversHandler
    {
        public static async Task Handle(
            DealObserversCommand command,
            MiniEnvDbContext db,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
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

            List<Guid> observerIds = command.ObserverIds
                .Where(x => x != deal.OwnerId)
                .Distinct()
                .ToList();

            if (await db.Users.Where(x => observerIds.Contains(x.Id))
                .CountAsync(cancellationToken) != observerIds.Count)
                throw new KeyNotFoundException("One or more users were not found.");

            deal.SetObservers(observerIds);
        }
    }
}
