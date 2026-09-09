using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace MiniEnv.Infrastructure.Extensions.DependencyInjection;

public static class RateLimitingServiceCollectionExtensions
{
    private const int PublicPermitLimit = 5;
    private const int AuthenticatedPermitLimit = 250;

    private static readonly TimeSpan RateLimitWindow =
        TimeSpan.FromMinutes(1);

    public static IServiceCollection AddApiRateLimiting(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter =
                PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    bool isPublic = httpContext.GetEndpoint()?
                        .Metadata
                        .GetMetadata<IAllowAnonymous>() is not null;

                    int permitLimit = isPublic
                        ? PublicPermitLimit
                        : AuthenticatedPermitLimit;

                    string partitionKey;

                    if (httpContext.User.Identity?.IsAuthenticated == true)
                    {
                        string userId =
                            httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                            ?? "unknown-user";

                        partitionKey = $"user:{userId}";
                    }
                    else
                    {
                        string ip =
                            httpContext.Connection.RemoteIpAddress?.ToString()
                            ?? "unknown-ip";

                        partitionKey = $"ip:{ip}";
                    }

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = RateLimitWindow,
                            QueueLimit = 0,
                            AutoReplenishment = true
                        });
                });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }

    public static WebApplication UseApiRateLimiting(
        this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseRateLimiter();

        return app;
    }
}
