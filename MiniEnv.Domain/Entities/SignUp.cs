using MiniEnv.Domain.Enums;

namespace MiniEnv.Domain.Entities
{
    public class SignUp
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = default!;
        public string TokenHash { get; private set; } = default!;
        public SignUpStatus Status { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private SignUp() { } // EF Core

        private SignUp(string email, string tokenHash, SignUpStatus status)
        {
            Id = Guid.NewGuid();
            Email = email;
            TokenHash = tokenHash;
            Status = status;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static SignUp Create(string email, string tokenHash, SignUpStatus status)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Argument 'email' cannot be empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new ArgumentException("Argument 'tokenHash' cannot be empty.", nameof(tokenHash));

            if (!Enum.IsDefined(status))
                throw new ArgumentException("Argument 'status' is invalid.", nameof(status));

            return new SignUp(email, tokenHash, status);
        }

        public void SetToVerified()
        {
            if (Status != SignUpStatus.Pending)
                throw new InvalidOperationException("Incompatible sign-up status.");

            Status = SignUpStatus.Verified;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void SetToActivated()
        {
            if (Status != SignUpStatus.Verified)
                throw new InvalidOperationException("Incompatible sign-up status.");

            Status = SignUpStatus.Activated;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void SetToExpired()
        {
            if (Status != SignUpStatus.Pending)
                throw new InvalidOperationException("Incompatible sign-up status.");

            Status = SignUpStatus.Expired;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
