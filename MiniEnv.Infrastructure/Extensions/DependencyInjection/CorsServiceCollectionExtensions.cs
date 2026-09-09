using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniEnv.Infrastructure.Extensions.DependencyInjection
{
    public static class CorsServiceCollectionExtensions
    {
        private const string CorsPolicyName = "MiniEnvCors";

        public static IServiceCollection AddCorsInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var origins = configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }

        public static WebApplication UseCorsInfrastructure(
            this WebApplication app)
        {
            app.UseCors(CorsPolicyName);

            return app;
        }
    }
}
