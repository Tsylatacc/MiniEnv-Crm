using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniEnv.Infrastructure.Common.Abstractions.Communication;
using MiniEnv.Infrastructure.Communication;
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

            services.Configure<MailerSendOptions>(configuration.GetSection("MailerSend"));
            services.AddHttpClient<EmailService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
