namespace MiniEnv.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public string TokenHash { get; private set; } = default!;

        public Guid UserId { get; private set; }
        public User User { get; private set; } = default!;

        public Guid? ReplacedByRefreshTokenId { get; private set; }

        public DateTimeOffset ExpiresAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? RevokedAt { get; private set; }


        private RefreshToken() { } // EF Core

        private RefreshToken(string tokenHash, Guid userId, DateTimeOffset expiresAt)
        {
            Id = Guid.NewGuid();
            TokenHash = tokenHash;
            UserId = userId;
            ExpiresAt = expiresAt;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static RefreshToken Create(string tokenHash, Guid userId, DateTimeOffset expiresAt)
        {
            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new ArgumentException("Argument 'tokenHash' cannot be empty.", nameof(tokenHash));

            if (userId == Guid.Empty)
                throw new ArgumentException("Argument 'userId' cannot be empty.", nameof(userId));

            if (expiresAt <= DateTimeOffset.UtcNow)
                throw new ArgumentException("Argument 'expiresAt' must be in the future.", nameof(expiresAt));

            return new RefreshToken(tokenHash, userId, expiresAt);
        }
        public void Revoke(Guid? replacedByRefreshTokenId)
        {
            if (RevokedAt is not null)
                throw new InvalidOperationException("Refresh token has already been revoked.");

            ReplacedByRefreshTokenId = replacedByRefreshTokenId;
            RevokedAt = DateTimeOffset.UtcNow;
        }

        public void Validate()
        {
            if (ExpiresAt < DateTimeOffset.UtcNow)
                throw new InvalidOperationException("Refresh token has expired.");

            if (RevokedAt is not null)
                throw new InvalidOperationException("Refresh token has already been revoked.");
        }
    }
}
