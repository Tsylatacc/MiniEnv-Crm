using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.Common.Abstractions.Caching;
using MiniEnv.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace MiniEnv.Infrastructure.Authorization
{
    public class UserTracking
    {
        private readonly MiniEnvDbContext _db;
        private readonly ICachingService _cachingService;
        public UserTracking(
            MiniEnvDbContext db,
            ICachingService cachingService)
        {
            _db = db;
            _cachingService = cachingService;
        }

        public async Task<UserPermissions> LoadPermissionsAsync(
            Guid tenantId,
            Guid userId,
            CancellationToken cancellationToken)
        {
            UserPermissions? cachedPermissions = await _cachingService.GetAsync<UserPermissions>(
                tenantId, userId, "user:permissions");
            if (cachedPermissions is not null)
                return cachedPermissions;

            var result = await _db.Users
                .IgnoreQueryFilters()
                .Where(x => x.Id == userId &&
                       x.TenantId == tenantId.ToString() &&
                       x.RoleId.HasValue)
                .Select(x => new
                {
                    RoleId = x.RoleId!.Value,
                    Permissions = x.Role!.RolePermissions
                        .Select(rp => new
                        {
                            rp.Permission.Name,
                            rp.AccessLevel
                        })
                        .ToList()
                })
                .SingleOrDefaultAsync(cancellationToken) ??
                    throw new UnauthorizedAccessException($"Authenticated user {userId} does not have a role nor permissions.");

            UserPermissions permissions = new(
                result.RoleId,
                result.Permissions.ToDictionary(
                    x => x.Name,
                    x => x.AccessLevel));

            await _cachingService.SetAsync<UserPermissions>(
                tenantId,
                userId,
                "user:permissions",
                permissions,
                TimeSpan.FromMinutes(15),
                When.Always);
            return permissions;
        }
    }
}
