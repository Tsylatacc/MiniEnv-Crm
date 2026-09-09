using MiniEnv.Infrastructure.Common.Abstractions.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace MiniEnv.Infrastructure.Caching
{
    public class MiniEnvCachingService(
        IConnectionMultiplexer redis) : ICachingService
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<T?> GetAsync<T>(
            Guid tenantId,
            Guid userId,
            string resource)
        {
            RedisValue value = await _database.StringGetAsync(
                $"minienv:tenant:{tenantId}:user:{userId}:{resource}");

            if (!value.HasValue)
                return default;

            return JsonSerializer.Deserialize<T>(value.ToString());
        }

        public async Task<bool> SetAsync<T>(
            Guid tenantId,
            Guid userId,
            string resource,
            T value,
            TimeSpan expiration,
            When when)
        {
            string serialized = JsonSerializer.Serialize(value);

            return await _database.StringSetAsync(
                $"minienv:tenant:{tenantId}:user:{userId}:{resource}",
                serialized,
                expiration,
                when);
        }

        public async Task RemoveAsync(
            Guid tenantId,
            Guid userId,
            string resource)
        {
            await _database.KeyDeleteAsync(
                $"minienv:tenant:{tenantId}:user:{userId}:{resource}");
        }
    }
}
