using MiniEnv.Infrastructure.Authentication;
using MiniEnv.Infrastructure.Authorization.Permissions;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using MiniEnv.Infrastructure.ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MiniEnv.Infrastructure.Extensions.DependencyInjection
{
    public static class HttpServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<ISignUpContext, SignupContext>();
            services.AddScoped<IRefreshContext, RefreshContext>();

            services.AddScoped<IJwtService, JwtService>();

            AddAuthentication(services, configuration);
            AddAuthorization(services);

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }

        private static void AddAuthentication(
            IServiceCollection services,
            IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Bearer:Issuer"],
                    ValidAudience = configuration["Jwt:Bearer:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["Jwt:Bearer:Key"]!))
                };
            })
            .AddJwtBearer("SignUp", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:SignUp:Issuer"],
                    ValidAudience = configuration["Jwt:SignUp:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["Jwt:SignUp:Key"]!))
                };
            });
        }

        public static IServiceCollection AddAuthorization(
        this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("Bearer")
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return services;
        }
    }
}
