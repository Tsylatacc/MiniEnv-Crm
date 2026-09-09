using MiniEnv.Domain.Enums;

namespace MiniEnv.Domain.SystemDefaults
{
    public sealed record StagePrototype(
        string Name,
        StageOutcome Outcome = StageOutcome.None);

}
