using MiniEnv.Domain.Enums;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Abstractions.Caching;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MiniEnv.Application.Features.Pipelines.Kanban
{
    public class KanbanHandler
    {
        public static async Task<KanbanResponse> Handle(
            KanbanCommand command,
            MiniEnvDbContext db,
            ICachingService cachingService,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
            string cacheResource = $"pipeline:kanban:{command.PipelineId}";
            KanbanDto? cachedDto = await cachingService.GetAsync<KanbanDto>(
                currentUser.TenantId,
                currentUser.UserId,
                cacheResource);
            if (cachedDto is not null)
                return new KanbanResponse(cachedDto);

            await currentUser.LoadPermissions(cancellationToken);
            KanbanDto? dto = currentUser.Permissions.PermissionsAccessLevel[Permissions.Names.ReadDeal] switch
            {
                AccessLevel.All => await db.Pipelines
                    .AsNoTracking()
                    .Where(x => x.Id == command.PipelineId)
                    .Select(x => new KanbanDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Stages = x.Stages
                            .OrderBy(x => x.Order)
                            .ThenByDescending(x => x.Id)
                            .Select(x => new KanbanStageDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Color = x.Color,
                                Deals = x.Deals
                                    .Where(x => x.DeletedAt == null)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.Id)
                                    .Skip(SystemConstants.KanbanSkip)
                                    .Take(SystemConstants.KanbanTake)
                                    .Select(x => new KanbanDealDto
                                    {
                                        Id = x.Id,
                                        Title = x.Title,
                                        OwnerId = x.OwnerId,
                                        OwnerName = x.Owner != null ? x.Owner.Name : null,
                                        CreatedAt = x.CreatedAt
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken),

                AccessLevel.Own => await db.Pipelines
                    .AsNoTracking()
                    .Where(x => x.Id == command.PipelineId)
                    .Select(x => new KanbanDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Stages = x.Stages
                            .OrderBy(x => x.Order)
                            .ThenByDescending(x => x.Id)
                            .Select(x => new KanbanStageDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Color = x.Color,
                                Deals = x.Deals
                                    .Where(x => x.DeletedAt == null)
                                    .Where(x => x.OwnerId == currentUser.UserId)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.Id)
                                    .Skip(SystemConstants.KanbanSkip)
                                    .Take(SystemConstants.KanbanTake)
                                    .Select(x => new KanbanDealDto
                                    {
                                        Id = x.Id,
                                        Title = x.Title,
                                        OwnerId = x.OwnerId,
                                        OwnerName = x.Owner != null ? x.Owner.Name : null,
                                        CreatedAt = x.CreatedAt
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken),

                _ => await db.Pipelines
                    .AsNoTracking()
                .Where(x => x.Id == command.PipelineId)
                .Select(x => new KanbanDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Stages = x.Stages
                        .OrderBy(x => x.Order)
                        .ThenByDescending(x => x.Id)
                        .Select(x => new KanbanStageDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Deals = { }
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken)
            };

            if (dto is not null)
                await cachingService.SetAsync(currentUser.TenantId,
                    currentUser.UserId,
                    cacheResource,
                    dto,
                    TimeSpan.FromMinutes(1),
                    When.Always);

            return new KanbanResponse(dto);
        }

        public sealed class KanbanDto
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = default!;
            public List<KanbanStageDto> Stages { get; init; } = [];
        }

        public sealed class KanbanStageDto
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = default!;
            public string Color { get; init; } = default!;
            public List<KanbanDealDto> Deals { get; init; } = [];
        }

        public sealed class KanbanDealDto
        {
            public Guid Id { get; init; }
            public string Title { get; init; } = default!;
            public Guid? OwnerId { get; init; }
            public string? OwnerName { get; init; }
            public DateTimeOffset CreatedAt { get; init; } = default!;
        }

    }
}
