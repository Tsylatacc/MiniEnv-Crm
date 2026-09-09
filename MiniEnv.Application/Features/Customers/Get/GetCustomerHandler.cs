using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Abstractions.Caching;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MiniEnv.Application.Features.Customers.Get
{
    public class GetCustomerHandler
    {
        public static async Task<GetCustomerResponse> Handle(
            GetCustomerCommand command,
            MiniEnvDbContext db,
            ICachingService cachingService,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
            string cacheResource = $"customer:{command.CustomerId}";
            CustomerDto? cachedDto = await cachingService.GetAsync<CustomerDto>(
                currentUser.TenantId,
                currentUser.UserId,
                cacheResource);
            if (cachedDto is not null)
                return new GetCustomerResponse(cachedDto);

            CustomerDto? dto = await db.Customers
                .AsNoTracking()
                .Where(x => x.Id == command.CustomerId)
                .Select(x => new CustomerDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedAt = x.CreatedAt,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dto is not null)
                await cachingService.SetAsync(currentUser.TenantId,
                    currentUser.UserId,
                    cacheResource,
                    dto,
                    TimeSpan.FromMinutes(20),
                    When.Always);

            return new GetCustomerResponse(dto);
        }

        public sealed class CustomerDto
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = default!;
            public DateTimeOffset CreatedAt { get; init; }
            public string? PhoneNumber { get; init; } = default!;
            public string? Email { get; init; } = default!;
        }
    }
}
