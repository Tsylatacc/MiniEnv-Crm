using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Application.Features.Customers.Patch
{
    public class PatchCustomerHandler
    {
        public static async Task Handle(
            PatchCustomerCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            await db.Customers
               .Where(x => x.Id == command.CustomerId)
               .ExecuteUpdateAsync(setters => setters
                   .SetProperty(x => x.Name, x => command.Name ?? x.Name)
                   .SetProperty(x => x.PhoneNumber, x => command.PhoneNumber ?? x.PhoneNumber)
                   .SetProperty(x => x.Email, x => command.Email ?? x.Email)
                   .SetProperty(x => x.UpdatedAt, _ => DateTimeOffset.UtcNow),
                   cancellationToken);
        }
    }
}
