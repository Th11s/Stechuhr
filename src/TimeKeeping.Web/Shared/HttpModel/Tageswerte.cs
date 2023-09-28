namespace Th11s.TimeKeeping.HttpModel
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
        StechTyp Stechtyp,
        bool IstNachbuchung,
        bool IstVorausbuchung,
        bool HatAnpassungen,
        bool IstEntfernt
        );

    public enum StechTyp
    {
        Arbeitsbeginn,
        Arbeitsende,
        Pausenbeginn,
        Pausenende
    }
}
