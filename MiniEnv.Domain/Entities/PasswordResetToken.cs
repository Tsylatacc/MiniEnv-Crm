namespace MiniEnv.Domain.Entities
{
    public class PasswordResetToken
    {
        public Guid Id { get; private set; }
        public string TokenHash { get; private set; } = default!;

        public Guid UserId { get; private set; }
        public User User { get; private set; } = default!;

        public DateTimeOffset ExpiresAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UsedAt { get; private set; }


        private PasswordResetToken() { } // EF Core

        private PasswordResetToken(string tokenHash, Guid userId, DateTimeOffset expiresAt)
        {
            Id = Guid.NewGuid();
            TokenHash = tokenHash;
            UserId = userId;
            ExpiresAt = expiresAt;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static PasswordResetToken Create(string tokenHash, Guid userId, DateTimeOffset expiresAt)
        {
            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new ArgumentException("Argument 'tokenHash' cannot be empty.", nameof(tokenHash));

            if (userId == Guid.Empty)
                throw new ArgumentException("Argument 'userId' cannot be empty.", nameof(userId));

            if (expiresAt <= DateTimeOffset.UtcNow)
                throw new ArgumentException("Argument 'expiresAt' must be in the future.", nameof(expiresAt));

            return new PasswordResetToken(tokenHash, userId, expiresAt);
        }

        public void Consume()
        {
            if (UsedAt is not null)
                throw new InvalidOperationException("Token has already been used.");

            if (ExpiresAt < DateTimeOffset.UtcNow)
                throw new InvalidOperationException("Token expired.");

            UsedAt = DateTimeOffset.UtcNow;
        }
    }
}
