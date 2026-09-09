using StackExchange.Redis;

namespace MiniEnv.Infrastructure.Common.Abstractions.Caching
{
    public interface ICachingService
    {
        Task<T?> GetAsync<T>(Guid tenantId,
            Guid userId,
            string resource);

        Task<bool> SetAsync<T>(Guid tenantId,
            Guid userId,
            string resource,
            T value,
            TimeSpan expiration,
            When when);

        Task RemoveAsync(Guid tenantId,
            Guid userId,
            string resource);
    }
}