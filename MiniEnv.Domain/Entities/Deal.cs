using MiniEnv.Domain.Enums;
using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class Deal : ITenanted
    {
        public Guid Id { get; private set; }
        public string Identifier { get; private set; }
        public string Title { get; private set; } = default!;
        public DealSource Source { get; private set; }
        public string Notes { get; private set; } = default!;

        public Guid StageId { get; private set; }
        public Stage Stage { get; private set; } = default!;

        private readonly List<DealCustomer> _customers = [];
        public IReadOnlyCollection<DealCustomer> Customers => _customers.AsReadOnly();

        public Guid? OwnerId { get; private set; }
        public User? Owner { get; private set; }

        private readonly List<DealObserver> _observers = [];
        public IReadOnlyCollection<DealObserver> Observers => _observers.AsReadOnly();

        public EntityOrigin Origin { get; private set; }
        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        private Deal() { } // EF Core

        private Deal(DealSource source, Guid stageId, EntityOrigin origin)
        {
            Id = Guid.NewGuid();
            Identifier = GenerateIdentifier();
            Title = GenerateTitle(Identifier);
            Source = source;
            StageId = stageId;
            Origin = origin;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Deal Create(DealSource source, Guid stageId, StageOutcome stageOutcome, EntityOrigin origin)
        {
            if (!Enum.IsDefined(source))
                throw new ArgumentException("Argument 'source' is invalid.", nameof(source));

            if (stageId == Guid.Empty)
                throw new ArgumentException("Argument 'stageId' cannot be empty.", nameof(stageId));

            if (!Enum.IsDefined(stageOutcome))
                throw new ArgumentException("Argument 'stageOutcome' is invalid.", nameof(stageOutcome));

            if (stageOutcome != StageOutcome.None)
                throw new InvalidOperationException($"It is not possible to create a deal within the Win/Loss stages.");

            if (!Enum.IsDefined(origin))
                throw new ArgumentException("Argument 'origin' is invalid.", nameof(origin));

            return new Deal(source, stageId, origin);
        }
        private static string GenerateIdentifier()
        {
            return Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        }

        private static string GenerateTitle(string identifier)
        {
            return $"Deal #{identifier}";
        }

        public void SetCustomers(IReadOnlyCollection<Guid> customerIds)
        {
            foreach (Guid customerId in customerIds.Distinct())
            {
                if (customerId == Guid.Empty)
                    throw new ArgumentException("Argument 'customerIds' cannot have empty values.", nameof(customerIds));
            }

            _customers.Clear();

            foreach (Guid customerId in customerIds.Distinct())
            {
                _customers.Add(DealCustomer.Create(Id, customerId));
            }

            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void SetOwner(Guid ownerId)
        {
            if (ownerId == Guid.Empty)
                throw new ArgumentException("Argument 'ownerId' cannot be empty.", nameof(ownerId));

            OwnerId = ownerId;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Delete()
        {
            if (DeletedAt.HasValue)
                return;

            DeletedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Move(Guid stageId)
        {
            if (stageId == Guid.Empty)
                throw new ArgumentException("Argument 'stageId' cannot be empty.", nameof(stageId));

            StageId = stageId;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Transfer(Guid newOwnerId)
        {
            if (newOwnerId == Guid.Empty)
                throw new ArgumentException("Argument 'newOwnerId' cannot be empty.", nameof(newOwnerId));

            OwnerId = newOwnerId;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void SetObservers(IReadOnlyCollection<Guid> observerIds)
        {
            foreach (Guid observerId in observerIds.Distinct())
            {
                if (observerId == Guid.Empty)
                    throw new ArgumentException("Argument 'observerIds' cannot have empty values.", nameof(observerIds));
            }

            _observers.Clear();

            foreach (Guid observerId in observerIds.Distinct())
            {
                _observers.Add(DealObserver.Create(Id, observerId));
            }

            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
