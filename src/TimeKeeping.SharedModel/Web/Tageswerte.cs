using Th11s.TimeKeeping.SharedModel.Primitives;

namespace Th11s.TimeKeeping.SharedModel.Web
{
    public record Tagessicht(
        DateOnly Datum,
        Stechzeit[] Zeitstempel,
        Tageswerte? Tageswerte
    );

    public record Tageswerte(
        TimeSpan? Arbeitszeit,
        TimeSpan? Pausenzeit,
        bool HatPausezeitminimum,

        TimeSpan Sollarbeitszeit,
        TimeSpan? Arbeitszeitgutschrift,
        TimeSpan Zeitsaldo,

        string[] Probleme,
        bool HatProbleme
        );

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
