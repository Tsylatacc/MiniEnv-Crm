using Microsoft.AspNetCore.Authorization;

namespace MiniEnv.Infrastructure.Authorization.Permissions
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public IReadOnlyCollection<string> Permissions { get; }

        public PermissionRequirement(params string[] permissions)
        {
            Permissions = permissions;
        }
    }
}

