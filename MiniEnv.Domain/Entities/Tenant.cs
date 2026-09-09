namespace MiniEnv.Domain.Entities
{
    public class Tenant
    {
        public string Id { get; private set; } = default!;
        public string? Name { get; private set; }

        public string PasswordRecoveryEmail { get; private set; } = default!;

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private Tenant() { } // EF Core

        private Tenant(Guid id, string passwordRecoveryEmail)
        {
            Id = id.ToString();
            PasswordRecoveryEmail = passwordRecoveryEmail;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Tenant Create(Guid id, string passwordRecoveryEmail)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Argument 'id' cannot be empty.", nameof(id));

            if (string.IsNullOrEmpty(passwordRecoveryEmail))
                throw new ArgumentException("Argument 'passwordRecoveryEmail' cannot be empty.", nameof(passwordRecoveryEmail));

            return new Tenant(id, passwordRecoveryEmail);
        }
    }
}
