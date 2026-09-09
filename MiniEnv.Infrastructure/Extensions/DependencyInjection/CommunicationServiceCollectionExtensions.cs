using MiniEnv.Infrastructure.Common.Abstractions.Communication;
using MiniEnv.Infrastructure.Communication;
using MiniEnv.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniEnv.Infrastructure.Extensions.DependencyInjection
{
    public static class CommunicationServiceCollectionExtensions
    {
        public static IServiceCollection AddCommunication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<FrontEndSettings>(configuration.GetSection("FrontEnd"));

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
