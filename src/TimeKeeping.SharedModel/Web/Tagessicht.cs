namespace Th11s.TimeKeeping.SharedModel.Web
{
    public record Tagessicht(
        DateOnly Datum,
        Stechzeit[] Zeitstempel,
        Tageswerte? Tageswerte
    );
}
