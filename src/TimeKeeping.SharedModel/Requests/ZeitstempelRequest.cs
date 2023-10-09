using Th11s.TimeKeeping.SharedModel.Primitives;

namespace Th11s.TimeKeeping.SharedModel.Requests;

public class ZeitstempelRequest
{
    public required DateOnly Datum { get; init; }
    public required Stempeltyp Stempeltyp { get; init; }

    public required DateTimeOffset Zeitstempel { get; init; }
}
