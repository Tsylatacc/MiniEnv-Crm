using MiniEnv.Domain.Enums;
using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class RolePermission : ITenanted
    {
        public Guid Id { get; private set; }

        public Guid RoleId { get; private set; }
        public Role Role { get; private set; } = default!;

        public Guid PermissionId { get; private set; }
        public Permission Permission { get; private set; } = default!;

        public AccessLevel AccessLevel { get; private set; }

        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }

        private RolePermission() { } // EF Core

        private RolePermission(Guid roleId, Guid permissionId, AccessLevel accessLevel)
        {
            Id = Guid.NewGuid();
            RoleId = roleId;
            PermissionId = permissionId;
            AccessLevel = accessLevel;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static RolePermission Create(Guid roleId, Guid permissionId, AccessLevel accessLevel)
        {
            if (roleId == Guid.Empty)
                throw new ArgumentException("Argument 'roleId' cannot be empty.", nameof(roleId));

            if (permissionId == Guid.Empty)
                throw new ArgumentException("Argument 'permissionId' cannot be empty.", nameof(permissionId));

            if (!Enum.IsDefined(accessLevel))
                throw new ArgumentException("Argument 'accessLevel' is invalid.", nameof(accessLevel));

            return new RolePermission(roleId, permissionId, accessLevel);
        }
    }
}
