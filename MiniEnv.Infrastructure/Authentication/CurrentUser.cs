using MiniEnv.Infrastructure.Authorization;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniEnv.Infrastructure.Authentication
{
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserTracking _userTracking;

        private UserPermissions? _permissions;

        public CurrentUser(
            IHttpContextAccessor context,
            UserTracking userTracking)
        {
            _context = context;
            _userTracking = userTracking;
        }

        private ClaimsPrincipal User =>
            _context.HttpContext?.User
            ?? throw new UnauthorizedAccessException("Unauthenticated user.");

        public Guid UserId =>
            Guid.Parse(
                User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? throw new UnauthorizedAccessException("UserId claim not found."));

        public Guid TenantId =>
            Guid.Parse(
                User.FindFirst(CustomClaimTypes.TenantId)?.Value
                ?? throw new UnauthorizedAccessException("TenantId claim not found."));

        public string Email =>
            User.FindFirst(JwtRegisteredClaimNames.Email)?.Value
            ?? throw new UnauthorizedAccessException("Email claim not found.");

        public UserPermissions Permissions => _permissions
            ?? throw new InvalidOperationException("User permissions have not been loaded.");

        public async Task LoadPermissions(CancellationToken cancellationToken)
        {
            if (_permissions is not null)
                return;
            _permissions = await _userTracking.LoadPermissionsAsync(TenantId, UserId, cancellationToken);
        }
    }
}
