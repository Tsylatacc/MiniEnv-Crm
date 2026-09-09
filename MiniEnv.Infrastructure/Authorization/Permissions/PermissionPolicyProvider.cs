using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MiniEnv.Infrastructure.Authorization.Permissions
{
    public sealed class PermissionPolicyProvider
        : DefaultAuthorizationPolicyProvider
    {
        private const string Prefix = "Permissions:";

        public PermissionPolicyProvider(
            IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(
            string policyName)
        {
            if (!policyName.StartsWith(Prefix))
            {
                return base.GetPolicyAsync(policyName);
            }

            var permissions = policyName[Prefix.Length..]
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes("Bearer")
                .RequireAuthenticatedUser()
                .AddRequirements(
                    new PermissionRequirement(permissions))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }
}