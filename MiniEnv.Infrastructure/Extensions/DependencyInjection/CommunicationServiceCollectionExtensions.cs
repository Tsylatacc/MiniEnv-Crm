using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniEnv.Infrastructure.Settings;

namespace MiniEnv.Infrastructure.Extensions.DependencyInjection
{
    public static class CommunicationServiceCollectionExtensions
    {
        public static IServiceCollection AddCommunication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<FrontEndSettings>(configuration.GetSection("FrontEnd"));

            return services;
        }
    }
}
