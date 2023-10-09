using Th11s.TimeKeeping.SharedModel.Primitives;

namespace Th11s.TimeKeeping.Data.Entities
{
    public record Zeiterfassungsaudit(
        string UserId,
        string? AnzeigeName,
        DateTimeOffset Zeitpunkt,

        AuditOperation Operation,
        DateTimeOffset? Wert
        );
}
