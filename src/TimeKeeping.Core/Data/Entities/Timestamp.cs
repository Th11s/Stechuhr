namespace Th11s.TimeKeeping.Data.Entities
{
    public class Timestamp
    {
        public DateTimeOffset At { get; set; }
        public StampType Type { get; set; }
    }

    public enum StampType
    {
        Undefined,
        WorkStart,
        WorkEnd,
        BreakStart,
        BreakEnd
    }
}
