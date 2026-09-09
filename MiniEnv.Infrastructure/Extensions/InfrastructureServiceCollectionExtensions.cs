using MiniEnv.Infrastructure.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniEnv.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddCaching(configuration);
            services.AddCommunication(configuration);

            return services;
        }
    }
}
