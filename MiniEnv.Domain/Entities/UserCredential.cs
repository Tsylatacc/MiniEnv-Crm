namespace MiniEnv.Domain.Entities
{
    public class UserCredential
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = default!;
        public string? PasswordHash { get; private set; }
        public string? Phone { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; } = default!;

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private UserCredential() { } // EF Core

        private UserCredential(string email, Guid userId)
        {
            Id = Guid.NewGuid();
            Email = email;
            UserId = userId;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static UserCredential Create(string email, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Argument 'email' cannot be empty.", nameof(email));

            if (userId == Guid.Empty)
                throw new ArgumentException("Argument 'userId' cannot be empty.", nameof(userId));

            return new UserCredential(email, userId);
        }

        public void SetPassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Argument 'newPasswordHash' cannot be empty.", nameof(newPasswordHash));

            PasswordHash = newPasswordHash;
        }
    }
}
