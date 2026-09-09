using MiniEnv.Domain.Enums;
using MiniEnv.Infrastructure.Common.Abstractions.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MiniEnv.Infrastructure.Authorization.Permissions
{
    public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ICurrentUser _currentUser;

        public PermissionAuthorizationHandler(
            ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated != true)
                return;

            await _currentUser.LoadPermissions(CancellationToken.None);
            if (requirement.Permissions.All(permission =>
                _currentUser.Permissions.PermissionsAccessLevel.TryGetValue(permission, out var accessLevel)
                && accessLevel != AccessLevel.None))
            {
                context.Succeed(requirement);
            }
        }
    }
}

