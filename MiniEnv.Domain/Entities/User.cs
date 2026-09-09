using MiniEnv.Domain.Enums;
using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class User : ITenanted
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string? PhoneNumber { get; private set; } = default!;
        public string? Email { get; private set; } = default!;

        public UserStatus Status { get; private set; }
        public bool IsAdmin { get; private set; } = false;

        public Guid? RoleId { get; private set; }
        public Role? Role { get; private set; } = default!;

        private readonly List<Deal> _deals = [];
        public IReadOnlyCollection<Deal> Deals => _deals.AsReadOnly();

        public UserCredential? Credential { get; private set; } = default!;

        private readonly List<Invitation> _invitations = [];
        public IReadOnlyCollection<Invitation> Invitations => _invitations.AsReadOnly();

        private readonly List<RefreshToken> _refreshTokens = [];
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public EntityOrigin Origin { get; private set; }
        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private User() { } // EF Core

        private User(string name, UserStatus status, EntityOrigin origin)
        {
            Id = Guid.NewGuid();
            Name = name;
            Status = status;
            Origin = origin;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static User Create(string name, UserStatus status, EntityOrigin origin)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument 'name' cannot be empty.", nameof(name));

            if (!Enum.IsDefined(status))
                throw new ArgumentException("Argument 'status' is invalid.", nameof(status));

            if (!Enum.IsDefined(origin))
                throw new ArgumentException("Argument 'origin' is invalid.", nameof(origin));

            return new User(name, status, origin);
        }

        public void SetCredential(UserCredential credential)
        {
            if (credential is null)
                throw new ArgumentException("Argument 'credential' cannot be null.", nameof(credential));

            Credential = credential;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        public void SetRole(Guid roleId)
        {
            if (roleId == Guid.Empty)
                throw new ArgumentException("Argument 'roleId' cannot be empty.", nameof(roleId));

            RoleId = roleId;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Activate()
        {
            if (Status == UserStatus.Disabled)
                throw new InvalidOperationException("Disabled user cannot be activated.");

            Status = UserStatus.Active;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Invitation? GetActiveInvitation()
        {
            return _invitations.FirstOrDefault(x => x.IsValid);
        }

    }
}
