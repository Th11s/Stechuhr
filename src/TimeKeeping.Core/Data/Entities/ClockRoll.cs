namespace Th11s.TimeKeeping.Data.Entities
{
    public class RollingClockEntry
    {
        public Guid Uuid { get; set; }

        public int UserId { get; set; }
        public int OrgUnitId { get; set; }


        public required Timestamp Stamp { get; set; }
        public AggregationInfo? Aggregation { get; set; }


        public AuditCollection ChangeTracking { get; set; } = new();
        public bool HasBeenModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
