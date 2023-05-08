namespace TimeKeeping.Web.Shared.HttpModel;

public class Entry
{
    public Entry()
    {
        
    }
    public Entry(EntryType type, DateTimeOffset at, Guid? uuid = null)
    {
        Type = type;
        At = at;
        Uuid = uuid ?? Guid.NewGuid();
    }

    public EntryType Type { get; set; }
    public DateTimeOffset At { get; set; }
    public Guid Uuid { get; }
}
