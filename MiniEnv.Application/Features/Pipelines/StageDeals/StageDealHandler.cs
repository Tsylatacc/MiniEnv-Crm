using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;
using MiniEnv.Domain.SystemDefaults;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace MiniEnv.Application.Features.Pipelines.StageDeals
{
    public class StageDealHandler
    {
        public static async Task<StageDealResponse> Handle(
            StageDealCommand command,
            MiniEnvDbContext db,
            ICurrentUser currentUser,
            CancellationToken cancellationToken)
        {
            await currentUser.LoadPermissions(cancellationToken);

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
                    return new StageDealResponse([]);
            }

            int total = await query.CountAsync(cancellationToken);
            IReadOnlyCollection<StageDealDto>? dealsDto = await query
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .Skip(command.Skip)
                .Take(SystemConstants.ListTake)
                .Select(x => new StageDealDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    OwnerId = x.OwnerId,
                    OwnerName = x.Owner != null ? x.Owner.Name : null,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new StageDealResponse(dealsDto ?? []);
        }

        public sealed class StageDealDto
        {
            public Guid Id { get; init; }
            public string Title { get; init; } = default!;
            public Guid? OwnerId { get; init; }
            public string? OwnerName { get; init; }
            public DateTimeOffset CreatedAt { get; init; } = default!;
        }
    }

}
