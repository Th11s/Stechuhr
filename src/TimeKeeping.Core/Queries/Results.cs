using JGUZDV.CQRS;

namespace Th11s.TimeKeeping.Queries
{
    internal static class Results
    {
        public static HandlerResult Empty { get; } = HandlerResult.Error("Command returned empty result.");
    }
}
