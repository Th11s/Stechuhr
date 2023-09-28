namespace Th11s.TimeKeeping.Data.Entities
{
    public record Stechzeit(
        DateOnly Datum,
        DateTimeOffset Zeitstempel,
        StechTyp Typ);

    public enum StechTyp
    {
        Arbeitsbeginn,
        Arbeitsende,
        Pausenbeginn,
        Pausenende
    }
}
