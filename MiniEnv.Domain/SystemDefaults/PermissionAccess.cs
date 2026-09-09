using MiniEnv.Domain.Enums;

namespace MiniEnv.Domain.SystemDefaults
{
    public sealed record PermissionAccess(
            PermissionIndex Permission,
            AccessLevel AccessLevel
        );
}
