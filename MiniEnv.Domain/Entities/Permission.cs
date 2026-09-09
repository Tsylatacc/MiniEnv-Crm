namespace MiniEnv.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;

        private readonly List<RolePermission> _rolePermissions = [];
        public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private Permission() { } // EF Core

        private Permission(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Permission Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument 'name' cannot be empty.", nameof(name));

            return new Permission(name);
        }
    }
}
