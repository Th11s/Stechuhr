namespace Th11s.TimeKeeping.SharedModel.Web
{
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
}
