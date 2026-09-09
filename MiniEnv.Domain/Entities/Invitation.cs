using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class Invitation : ITenanted
    {
        public Guid Id { get; private set; }

        public string TokenHash { get; private set; } = default!;

        public Guid UserId { get; private set; }
        public User User { get; private set; } = default!;

        public string TenantId { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }

        public DateTimeOffset? AcceptedAt { get; private set; }
        public DateTimeOffset? RevokedAt { get; private set; }


        private Invitation() { } // EF Core

        private Invitation(string tokenHash, Guid userId, DateTimeOffset expiresAt)
        {
            Id = Guid.NewGuid();
            TokenHash = tokenHash;
            UserId = userId;
            CreatedAt = DateTimeOffset.UtcNow;
            ExpiresAt = expiresAt;
        }


        public static Invitation Create(
            string tokenHash,
            Guid userId,
            DateTimeOffset expiresAt)
        {
            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new ArgumentException("Argument 'tokenHash' cannot be empty.", nameof(tokenHash));

            if (userId == Guid.Empty)
                throw new ArgumentException("Argument 'userId' cannot be empty.", nameof(userId));

            if (expiresAt <= DateTimeOffset.UtcNow)
                throw new ArgumentException("Argument 'expiresAt' must be in the future.", nameof(expiresAt));

            return new Invitation(
                tokenHash,
                userId,
                expiresAt);
        }


        public void Accept()
        {
            if (IsExpired)
                throw new InvalidOperationException("Invitation has expired.");

            if (IsAccepted)
                throw new InvalidOperationException("Invitation has already been accepted.");

            if (IsRevoked)
                throw new InvalidOperationException("Invitation has been revoked.");

            AcceptedAt = DateTimeOffset.UtcNow;

            User.Activate();
        }

        public void Revoke()
        {
            if (IsExpired)
                throw new InvalidOperationException("Expired invitation cannot be revoked.");

            if (IsAccepted)
                throw new InvalidOperationException("Accepted invitation cannot be revoked.");

            if (IsRevoked)
                throw new InvalidOperationException("Invitation has already been revoked.");

            RevokedAt = DateTimeOffset.UtcNow;
        }


        private bool IsExpired => ExpiresAt < DateTimeOffset.UtcNow;
        private bool IsAccepted => AcceptedAt is not null;
        private bool IsRevoked => RevokedAt is not null;

        public bool IsValid =>
            !IsAccepted &&
            !IsRevoked &&
            !IsExpired;
    }
}