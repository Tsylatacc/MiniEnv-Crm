using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class DealCustomer : ITenanted
    {
        public Guid Id { get; private set; }

        public Guid DealId { get; private set; }
        public Deal Deal { get; private set; } = default!;

        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;

        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }

        private DealCustomer() { } // EF Core
        private DealCustomer(Guid dealId, Guid customerId)
        {
            Id = Guid.NewGuid();
            DealId = dealId;
            CustomerId = customerId;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static DealCustomer Create(Guid dealId, Guid customerId)
        {
            if (dealId == Guid.Empty)
                throw new ArgumentException("Argument 'dealId' cannot be empty.", nameof(dealId));

            if (customerId == Guid.Empty)
                throw new ArgumentException("Argument 'customerId' cannot be empty.", nameof(customerId));

            return new DealCustomer(dealId, customerId);
        }
    }
}
