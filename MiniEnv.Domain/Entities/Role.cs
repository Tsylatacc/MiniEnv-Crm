using MiniEnv.Domain.Enums;
using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class Role : ITenanted
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;

        private readonly List<User> _users = [];
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        private readonly List<RolePermission> _rolePermissions = [];
        public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

        public EntityOrigin Origin { get; private set; }
        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private Role() { } // EF Core

        private Role(string name, EntityOrigin origin)
        {
            Id = Guid.NewGuid();
            Name = name;
            Origin = origin;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Role Create(string name, EntityOrigin origin)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument 'name' cannot be empty.", nameof(name));

            if (!Enum.IsDefined(origin))
                throw new ArgumentException("Argument 'origin' is invalid.", nameof(origin));

            return new Role(name, origin);
        }

        public void SetPermissions(IReadOnlyDictionary<Guid, AccessLevel> permissionsAccess)
        {
            _rolePermissions.Clear();

            foreach (var (permissionId, accessLevel) in permissionsAccess.Distinct())
            {
                if (permissionId == Guid.Empty)
                    throw new ArgumentException("'PermissionId' cannot be empty.", nameof(permissionId));

                if (!Enum.IsDefined(accessLevel))
                    throw new ArgumentException("'AccessLevel' is invalid.", nameof(accessLevel));

                _rolePermissions.Add(RolePermission.Create(Id, permissionId, accessLevel));
            }
        }
    }
}
