using System;
using System.Text.Json.Serialization;

namespace TimeKeeping.Web.Shared.HttpModel;

public class Entry
{
    public Entry()
    {
        Uuid = Guid.NewGuid();
    }

    public Entry(EntryType type, DateTimeOffset at)
    {
        Type = type;
        At = at;
        Uuid = Guid.NewGuid();
    }

    [JsonConstructor]
    public Entry(EntryType type, DateTimeOffset at, Guid uuid)
    {
        Type = type;
        At = at;
        Uuid = uuid == Guid.Empty ? Guid.NewGuid() : uuid;
    }

    public EntryType Type { get; set; }
    public DateTimeOffset At { get; set; }
    public Guid Uuid { get; }
}
