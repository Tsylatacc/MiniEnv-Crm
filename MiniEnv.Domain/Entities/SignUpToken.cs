namespace MiniEnv.Domain.Entities
{
    public class SignUpToken
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = default!;
        public string TokenHash { get; private set; } = default!;

        public DateTimeOffset? UsedAt { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private SignUpToken() { } // EF Core

        private SignUpToken(string email, string tokenHash, DateTimeOffset expiresAt)
        {
            Id = Guid.NewGuid();
            Email = email;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static SignUpToken Create(string email, string tokenHash, DateTimeOffset expiresAt)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Argument 'email' cannot be empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new ArgumentException("Argument 'tokenHash' cannot be empty.", nameof(tokenHash));
            
            if (expiresAt <= DateTimeOffset.UtcNow)
                throw new ArgumentException("Argument 'expiresAt' must be in the future.", nameof(expiresAt));
            
            return new SignUpToken(email, tokenHash, expiresAt);
        }

        public void SetExpiration(DateTimeOffset expiresAt)
        {
            if (ExpiresAt <= DateTimeOffset.UtcNow)
                throw new InvalidOperationException("Sign-up is not expired yet.");

            ExpiresAt = expiresAt;
        }

        public void SetUsed()
        {
            if (UsedAt is not null)
                return;
            
             UsedAt = DateTimeOffset.UtcNow;
        }
    }
}
