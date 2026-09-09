namespace MiniEnv.Domain.SystemDefaults
{
    public class DefaultStages
    {
        private static readonly Dictionary<StageIndex, string> _names = new()
        {
            [StageIndex.Lead] = "Lead",
            [StageIndex.Qualified] = "Qualified",
            [StageIndex.Proposal] = "Proposal",
            [StageIndex.Negotiation] = "Negotiation",
            [StageIndex.Won] = "Won",
            [StageIndex.Lost] = "Lost",
        };

        private static readonly Dictionary<StageIndex, StagePrototype> _stages = new()
        {
            [StageIndex.Lead] = new StagePrototype(_names[StageIndex.Lead]),
            [StageIndex.Qualified] = new StagePrototype(_names[StageIndex.Qualified]),
            [StageIndex.Proposal] = new StagePrototype(_names[StageIndex.Proposal]),
            [StageIndex.Negotiation] = new StagePrototype(_names[StageIndex.Negotiation]),
            [StageIndex.Won] = new StagePrototype(_names[StageIndex.Won], Enums.StageOutcome.Won),
            [StageIndex.Lost] = new StagePrototype(_names[StageIndex.Lost], Enums.StageOutcome.Lost),
        };

        public static IReadOnlyDictionary<StageIndex, StagePrototype> Collection = _stages.AsReadOnly();
    }
}
