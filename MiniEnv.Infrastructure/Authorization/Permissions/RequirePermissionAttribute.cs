using Microsoft.AspNetCore.Authorization;

namespace MiniEnv.Infrastructure.Authorization.Permissions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class RequirePermissionAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "Permissions:";

        public RequirePermissionAttribute(params string[] permissions)
        {
            Policy = PolicyPrefix + string.Join(",", permissions);
        }
    }
}

