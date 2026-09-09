using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;

namespace MiniEnv.Domain.SystemDefaults
{
    public class DefaultPipelines
    {
        private static readonly Dictionary<PipelineIndex, string> _names = new()
        {
            [PipelineIndex.Sales] = "Sales",
        };

        private static readonly IReadOnlyDictionary<StageIndex, StagePrototype> _stages = DefaultStages.Collection;
        private static readonly Dictionary<PipelineIndex, IReadOnlyCollection<StagePrototype>> _pipelines = new()
        {
            [PipelineIndex.Sales] = [
                _stages[StageIndex.Lead],
                _stages[StageIndex.Qualified],
                _stages[StageIndex.Proposal],
                _stages[StageIndex.Negotiation],
                _stages[StageIndex.Won],
                _stages[StageIndex.Lost]
                ]
        };

        public IReadOnlyDictionary<PipelineIndex, IReadOnlyCollection<StagePrototype>> Collection = _pipelines.AsReadOnly();

        public static IReadOnlyCollection<Pipeline> CreateForTenant()
        {
            List<Pipeline> pipelines = [];

            foreach (var (pipelineIndex, stages) in _pipelines)
            {
                Pipeline pipeline = Pipeline.Create(_names[pipelineIndex], EntityOrigin.System);
                foreach (StagePrototype stage in stages)
                {
                    pipeline.AddStage(
                        stage.Name,
                        stage.Outcome is StageOutcome.Won or StageOutcome.Lost
                            ? EntityOrigin.System
                            : EntityOrigin.Custom,
                        stage.Outcome);
                }
                pipelines.Add(pipeline);
            }
            return pipelines;
        }
    }
}
