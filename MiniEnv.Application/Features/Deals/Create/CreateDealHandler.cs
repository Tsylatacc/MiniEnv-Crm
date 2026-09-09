using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Deals.Create
{
    public class CreateDealHandler
    {
        public static async Task<CreateDealResponse> Handle(
            CreateDealCommand command,
            MiniEnvDbContext db,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {

            Stage stage = await db.Stages.FindAsync(command.StageId, cancellationToken) ??
                throw new KeyNotFoundException($"Stage {command.StageId} not found.");

            Deal deal = Deal.Create(DealSource.Manual, command.StageId, stage.Outcome, EntityOrigin.Custom);
            deal.SetOwner(currentUser.UserId);

            List<Guid>? customerIds = command.CustomerIds?
               .Distinct()
               .ToList();
            if (customerIds is not null)
            {
                if (await db.Customers.Where(x => customerIds.Contains(x.Id))
                .CountAsync(cancellationToken) != customerIds.Count)
                    throw new KeyNotFoundException("One or more customers were not found.");
                deal.SetCustomers(customerIds);
            }

            await db.Deals.AddAsync(deal, cancellationToken);

            return new CreateDealResponse(deal.Id);
        }
    }
}
