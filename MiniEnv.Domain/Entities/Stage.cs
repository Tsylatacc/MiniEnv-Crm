using MiniEnv.Domain.Enums;
using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class Stage : ITenanted
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Color { get; private set; } = "#000000";
        public StageOutcome Outcome { get; private set; }
        public int Order { get; private set; }

        public Guid PipelineId { get; private set; }

        private readonly List<Deal> _deals = [];
        public IReadOnlyCollection<Deal> Deals => _deals.AsReadOnly();

        public EntityOrigin Origin { get; private set; }
        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private Stage() { } // EF Core

        private Stage(string name, int order, Guid pipelineId, EntityOrigin origin, StageOutcome outcome = StageOutcome.None)
        {
            Id = Guid.NewGuid();
            Name = name;
            Order = order;
            Outcome = outcome;
            PipelineId = pipelineId;
            Origin = origin;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Stage Create(string name, int order, Guid pipelineId, EntityOrigin origin, StageOutcome outcome = StageOutcome.None)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument 'name' cannot be empty.", nameof(name));

            if (order < 0)
                throw new ArgumentException("Argument 'order' cannot be negative.", nameof(order));

            if (!Enum.IsDefined(outcome))
                throw new ArgumentException("Argument 'outcome' is invalid.", nameof(outcome));

            if (pipelineId == Guid.Empty)
                throw new ArgumentException("Argument 'pipelineId' cannot be empty.", nameof(pipelineId));

            if (!Enum.IsDefined(origin))
                throw new ArgumentException("Argument 'origin' is invalid.", nameof(origin));

            return new Stage(name, order, pipelineId, origin, outcome);
        }

        public void SetOrder(int order)
        {
            if (order < 0)
                throw new ArgumentException("Argument 'order' cannot be negative.", nameof(order));

            Order = order;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

    }
}
