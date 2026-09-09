using MiniEnv.Domain.Enums;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Abstractions.Caching;
using MiniEnv.Infrastructure.Common.Exceptions;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MiniEnv.Application.Features.Deals.Get
{
    public class GetDealHandler
    {
        public static async Task<GetDealResponse> Handle(
            GetDealCommand command,
            MiniEnvDbContext db,
            ICachingService cachingService,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
            string cacheResource = $"deal:{command.DealId}";
            DealDto? cachedDto = await cachingService.GetAsync<DealDto>(
                currentUser.TenantId,
                currentUser.UserId,
                cacheResource);
            if (cachedDto is not null)
                return new GetDealResponse(cachedDto);

            await currentUser.LoadPermissions(cancellationToken);
            DealDto? dto = currentUser.Permissions.PermissionsAccessLevel[Permissions.Names.ReadDeal] switch
            {
                AccessLevel.All => await db.Deals
                    .AsNoTracking()
                    .Where(x =>
                        x.Id == command.DealId &&
                        x.DeletedAt == null)
                    .Select(x => new DealDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Source = x.Source,
                        Notes = x.Notes,

                        Owner = x.Owner == null
                            ? null
                            : new DealOwnerDto
                            {
                                Id = x.Owner.Id,
                                Name = x.Owner.Name
                            },

                        Stage = new DealStageDto
                        {
                            Id = x.Stage.Id,
                            Name = x.Stage.Name,
                            Color = x.Stage.Color
                        },

                        Customers = x.Customers
                            .Select(dc => new DealCustomerDto
                            {
                                Id = dc.Customer.Id,
                                Name = dc.Customer.Name
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken),

                AccessLevel.Own => await db.Deals
                    .AsNoTracking()
                    .Where(x =>
                        x.Id == command.DealId &&
                        x.OwnerId == currentUser.UserId &&
                        x.DeletedAt == null)
                    .Select(x => new DealDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Source = x.Source,
                        Notes = x.Notes,

                        Owner = x.Owner == null
                            ? null
                            : new DealOwnerDto
                            {
                                Id = x.Owner.Id,
                                Name = x.Owner.Name
                            },

                        Stage = new DealStageDto
                        {
                            Id = x.Stage.Id,
                            Name = x.Stage.Name,
                            Color = x.Stage.Color
                        },

                        Customers = x.Customers
                            .Select(dc => new DealCustomerDto
                            {
                                Id = dc.Customer.Id,
                                Name = dc.Customer.Name
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

            return new GetDealResponse(dto);
        }

        public sealed class DealDto
        {
            public Guid Id { get; init; }
            public string Title { get; init; } = default!;
            public DealSource Source { get; init; }
            public string Notes { get; init; } = default!;
            public DealStageDto Stage { get; init; } = default!;
            public DealOwnerDto? Owner { get; init; }
            public List<DealCustomerDto> Customers { get; init; } = [];

        }

        public sealed class DealStageDto
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = default!;
            public string Color { get; init; } = default!;
        }

        public sealed class DealOwnerDto
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = default!;
        }

        public sealed class DealCustomerDto
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = default!;
        }
    }
}
