namespace Th11s.TimeKeeping.Data.Entities
{
    public class AuditCollection
    {
        public List<AuditEntry> Audits { get; set; } = new();
    }
    
    public record AuditEntry(
        DateTimeOffset At,
        string Command,
        string Data,

        int UserId,
        string UserName
        );
    }
}
