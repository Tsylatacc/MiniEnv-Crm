using MiniEnv.Domain.Enums;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Abstractions.Caching;
using MiniEnv.Infrastructure.Common.Exceptions;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MiniEnv.Application.Features.Customers.GetDeals
{
    public class GetCustomerDealsHandler
    {
        public static async Task<GetCustomerDealsResponse> Handle(
            GetCustomerDealsCommand command,
            MiniEnvDbContext db,
            ICachingService cachingService,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
            string cacheResource = $"customer:deals:{command.CustomerId}";
            CustomerDealsDto? cachedDto = await cachingService.GetAsync<CustomerDealsDto>(
                currentUser.TenantId,
                currentUser.UserId,
                cacheResource);
            if (cachedDto is not null)
                return new GetCustomerDealsResponse(cachedDto);

            await currentUser.LoadPermissions(cancellationToken);
            CustomerDealsDto? dto = currentUser.Permissions.PermissionsAccessLevel[Permissions.Names.ReadDeal] switch
            {
                AccessLevel.All => await db.Customers
                    .AsNoTracking()
                    .Where(x => x.Id == command.CustomerId)
                    .Select(x => new CustomerDealsDto
                    {
                        Deals = x.Deals
                            .Where(x => x.Deal.DeletedAt == null)
                            .OrderByDescending(x => x.Deal.CreatedAt)
                            .ThenByDescending(x => x.Deal.Id)
                            .Select(x => new CustomerDealDto
                            {
                                Id = x.Deal.Id,
                                Title = x.Deal.Title,
                                Owner = x.Deal.Owner == null
                                    ? null
                                    : new CustomerOwnerDto
                                    {
                                        Id = x.Deal.Owner.Id,
                                        Name = x.Deal.Owner.Name
                                    },
                                Stage = new CustomerStageDto
                                {
                                    Id = x.Deal.Stage.Id,
                                    Name = x.Deal.Stage.Name,
                                    Color = x.Deal.Stage.Color
                                }
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken),

                AccessLevel.Own => await db.Customers
                    .AsNoTracking()
                    .Where(x => x.Id == command.CustomerId)
                    .Select(x => new CustomerDealsDto
                    {
                        Deals = x.Deals
                            .Where(x => x.Deal.DeletedAt == null)
                            .Where(x => x.Deal.OwnerId == currentUser.UserId)
                            .OrderByDescending(x => x.Deal.CreatedAt)
                            .ThenByDescending(x => x.Deal.Id)
                            .Select(x => new CustomerDealDto
                            {
                                Id = x.Deal.Id,
                                Title = x.Deal.Title,
                                Owner = x.Deal.Owner == null
                                    ? null
                                    : new CustomerOwnerDto
                                    {
                                        Id = x.Deal.Owner.Id,
                                        Name = x.Deal.Owner.Name
                                    },
                                Stage = new CustomerStageDto
                                {
                                    Id = x.Deal.Stage.Id,
                                    Name = x.Deal.Stage.Name,
                                    Color = x.Deal.Stage.Color
                                }
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken),

                _ => throw new ForbiddenException()
            };

            if (dto is not null)
                await cachingService.SetAsync(currentUser.TenantId,
                    currentUser.UserId,
                    cacheResource,
                    dto,
                    TimeSpan.FromMinutes(5),
                    When.Always);

            return new GetCustomerDealsResponse(dto);
        }

        public sealed class CustomerDealsDto
        {
            public List<CustomerDealDto> Deals { get; init; } = [];
        }

        public sealed class CustomerDealDto
        {
            public Guid Id { get; init; }
            public string Title { get; init; } = default!;
            public CustomerOwnerDto? Owner { get; init; }
            public CustomerStageDto Stage { get; init; } = default!;
        }

        public sealed class CustomerOwnerDto
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = default!;
        }

        public sealed class CustomerStageDto
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = default!;
            public string Color { get; init; } = default!;
        }
    }
}
