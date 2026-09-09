using MiniEnv.Domain.Enums;
using JasperFx.MultiTenancy;

namespace MiniEnv.Domain.Entities
{
    public class Pipeline : ITenanted
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;

        private readonly List<Stage> _stages = [];
        public IReadOnlyCollection<Stage> Stages => _stages.AsReadOnly();

        public EntityOrigin Origin { get; private set; }
        public string TenantId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        private Pipeline() { } // EF Core

        private Pipeline(string name, EntityOrigin origin)
        {
            Id = Guid.NewGuid();
            Name = name;
            Origin = origin;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Pipeline Create(string name, EntityOrigin origin)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument 'name' cannot be empty.", nameof(name));

            if (!Enum.IsDefined(origin))
                throw new ArgumentException("Argument 'origin' is invalid.", nameof(origin));

            return new Pipeline(name, origin);
        }

        public Stage AddStage(string name, EntityOrigin origin, StageOutcome outcome = StageOutcome.None)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument 'name' cannot be empty.", nameof(name));

            if (!Enum.IsDefined(origin))
                throw new ArgumentException("Argument 'origin' is invalid.", nameof(origin));

            Stage stage = Stage.Create(name, _stages.Count, Id, origin, outcome);
            _stages.Add(stage);
            UpdatedAt = DateTimeOffset.UtcNow;
            return stage;
        }

        public void RemoveStage(Guid stageId)
        {
            Stage? stage = _stages.FirstOrDefault(x => x.Id == stageId);
            if (stage is null)
                throw new ArgumentException("Stage not found.", nameof(stageId));

            _stages.Remove(stage);
            ReorderStages();
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        public void ReorderStages(IReadOnlyCollection<Guid> stageIds)
        {
            if (stageIds.Count != _stages.Count)
                throw new ArgumentException("All stages must be provided.", nameof(stageIds));

            if (stageIds.Distinct().Count() != stageIds.Count)
                throw new ArgumentException("Stage ids must be unique.", nameof(stageIds));

            Dictionary<Guid, Stage>? stagesById = _stages.ToDictionary(x => x.Id);
            if (stageIds.Any(id => !stagesById.ContainsKey(id)))
                throw new ArgumentException("One or more stage ids are invalid.", nameof(stageIds));

            _stages.Clear();
            int order = 0;

            foreach (Guid stageId in stageIds)
            {
                Stage stage = stagesById[stageId];
                stage.SetOrder(order++);
                _stages.Add(stage);
            }
            ;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        private void ReorderStages()
        {
            for (var i = 0; i < _stages.Count; i++)
                _stages[i].SetOrder(i);
        }
    }
}
