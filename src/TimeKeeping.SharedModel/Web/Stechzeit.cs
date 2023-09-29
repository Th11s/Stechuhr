using Th11s.TimeKeeping.SharedModel.Primitives;

namespace Th11s.TimeKeeping.SharedModel.Web
{
    public record Stechzeit(
        Guid Uuid,
        DateTimeOffset Zeitstempel,
        Stempeltyp Stechtyp,
        bool IstNachbuchung,
        bool IstVorausbuchung,
        bool HatAnpassungen,
        bool IstEntfernt
        );
}
