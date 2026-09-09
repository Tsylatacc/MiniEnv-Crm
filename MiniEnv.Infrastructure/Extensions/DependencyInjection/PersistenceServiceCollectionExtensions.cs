using MiniEnv.Infrastructure.Authorization;
using MiniEnv.Infrastructure.Persistence;
using MiniEnv.Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.EntityFrameworkCore;

namespace MiniEnv.Infrastructure.Extensions.DependencyInjection
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string dbConnection = configuration.GetConnectionString("db")
                ?? throw new InvalidOperationException("Connection string 'db' not found.");

            services.AddDbContextWithWolverineManagedConjoinedTenancy<MiniEnvDbContext>(
                (options, dbZone) => options.UseNpgsql(
                    dbZone.Value,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }
                )
            );

            services.AddScoped<DatabaseSeeder>();
            services.AddScoped<UserTracking>();

            return services;
        }
    }
}
