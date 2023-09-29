using Th11s.TimeKeeping.SharedModel.Primitives;

namespace TimeKeeping.Web.Shared.HttpModel;

public class Entry
{
    public Entry()
    {
        
    }
    public Entry(Stempeltyp type, DateTimeOffset at, Guid? uuid = null)
    {
        Type = type;
        At = at;
        Uuid = uuid ?? Guid.NewGuid();
    }

    public Stempeltyp Type { get; set; }
    public DateTimeOffset At { get; set; }
    public Guid Uuid { get; }
}
