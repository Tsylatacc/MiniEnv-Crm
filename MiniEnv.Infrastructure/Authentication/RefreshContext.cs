using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace MiniEnv.Infrastructure.Authentication
{
    public sealed class RefreshContext : IRefreshContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshContext(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string RefreshToken =>
            _httpContextAccessor.HttpContext?
                .Request
                .Cookies["refresh_token"]
            ?? throw new UnauthorizedAccessException("Refresh token not found.");
    }
}