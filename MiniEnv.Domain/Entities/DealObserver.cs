using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class DealObserver : ITenanted
    {
        public Guid Id { get; private set; }

        public Guid DealId { get; private set; }
        public Deal Deal { get; private set; } = default!;

        public Guid ObserverId { get; private set; }
        public User Observer { get; private set; } = default!;

        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }

        private DealObserver() { } // EF Core
        private DealObserver(Guid dealId, Guid observerId)
        {
            Id = Guid.NewGuid();
            DealId = dealId;
            ObserverId = observerId;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static DealObserver Create(Guid dealId, Guid observerId)
        {
            if (dealId == Guid.Empty)
                throw new ArgumentException("Argument 'dealId' cannot be empty.", nameof(dealId));

            if (observerId == Guid.Empty)
                throw new ArgumentException("Argument 'observerId' cannot be empty.", nameof(observerId));

            return new DealObserver(dealId, observerId);
        }
    }
}
