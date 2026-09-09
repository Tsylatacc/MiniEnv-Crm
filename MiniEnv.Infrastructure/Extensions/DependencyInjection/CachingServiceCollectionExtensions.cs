using MiniEnv.Infrastructure.Caching;
using MiniEnv.Infrastructure.Common.Abstractions.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace MiniEnv.Infrastructure.Extensions.DependencyInjection
{
    public static class CachingServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string redisConnection = configuration.GetConnectionString("redis")
                ?? throw new InvalidOperationException("Connection string 'redis' not found.");

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));

            services.AddScoped<ICachingService, MiniEnvCachingService>();

            return services;
        }
    }
}
