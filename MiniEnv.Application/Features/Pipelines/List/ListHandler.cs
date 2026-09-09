using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Abstractions.Caching;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MiniEnv.Application.Features.Pipelines.List
{
    public class ListHandler
    {
        public static async Task<ListResponse> Handle(
            ListCommand command,
            MiniEnvDbContext db,
            ICachingService cachingService,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
            string cacheResource = $"pipeline:list";
            ListResponse? cachedDto = await cachingService.GetAsync<ListResponse>(
                currentUser.TenantId,
                currentUser.UserId,
                cacheResource);
            if (cachedDto is not null)
                return cachedDto;

            await currentUser.LoadPermissions(cancellationToken);

            int skip = command.Skip ?? 0;

            IQueryable<Deal> query = db.Deals
                .AsNoTracking()
                .Where(x => x.DeletedAt == null);

            switch (currentUser.Permissions.PermissionsAccessLevel[Permissions.Names.ReadDeal])
            {
                case AccessLevel.All:
                    break;

                case AccessLevel.Own:
                    query = query.Where(x => x.OwnerId == currentUser.UserId);
                    break;

                default:
                    return new ListResponse(null, 0);
            }

            int total = await query.CountAsync(cancellationToken);
            IReadOnlyCollection<ListDealDto>? dealsDto = await query
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .Skip(skip)
                .Take(SystemConstants.ListTake)
                .Select(x => new ListDealDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    OwnerId = x.OwnerId,
                    OwnerName = x.Owner != null ? x.Owner.Name : null,
                    StageId = x.StageId,
                    StageName = x.Stage.Name,
                    StageColor = x.Stage.Color,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            ListResponse response = new ListResponse(dealsDto, total);

            if (dealsDto is not null)
                await cachingService.SetAsync(currentUser.TenantId,
                    currentUser.UserId,
                    cacheResource,
                    response,
                    TimeSpan.FromMinutes(1),
                    When.Always);

            return response;
        }

        public sealed class ListDealDto
        {
            public Guid Id { get; init; }
            public string Title { get; init; } = default!;
            public Guid? OwnerId { get; init; }
            public string? OwnerName { get; init; }
            public Guid StageId { get; init; }
            public string StageName { get; init; } = default!;
            public string StageColor { get; init; } = default!;
            public DateTimeOffset CreatedAt { get; init; }
        }
    }

}
