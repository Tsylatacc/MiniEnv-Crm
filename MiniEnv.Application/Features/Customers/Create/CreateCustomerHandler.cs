using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Persistence;

namespace MiniEnv.Application.Features.Customers.Create
{
    public class CreateCustomerHandler
    {
        public static async Task<CreateCustomerResponse> Handle(
            CreateCustomerCommand command,
            MiniEnvDbContext db,
            CancellationToken cancellationToken)
        {
            Customer customer = Customer.Create(command.Name, EntityOrigin.Custom);
            await db.AddAsync(customer, cancellationToken);
            return new CreateCustomerResponse(customer.Id);
        }
    }
}
