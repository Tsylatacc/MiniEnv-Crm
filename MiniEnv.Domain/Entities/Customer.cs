using MiniEnv.Domain.Enums;
using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class Customer : ITenanted
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string? PhoneNumber { get; private set; }
        public string? Email { get; private set; }

        private readonly List<DealCustomer> _deals = [];
        public IReadOnlyCollection<DealCustomer> Deals => _deals.AsReadOnly();

        public EntityOrigin Origin { get; private set; }
        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private Customer() { } // EF Core

        private Customer(string name, EntityOrigin origin)
        {
            Id = Guid.NewGuid();
            Name = name;
            Origin = origin;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Customer Create(string name, EntityOrigin origin)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument 'name' cannot be empty.", nameof(name));

            if (!Enum.IsDefined(origin))
                throw new ArgumentException("Argument 'origin' is invalid.", nameof(origin));

            return new Customer(name, origin);
        }
    }
}
