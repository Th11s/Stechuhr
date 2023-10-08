namespace Th11s.TimeKeeping.SharedModel.Web
{
    public record Monatssicht(
        int Jahr, int Monat,
        Tageswerte[] Tageswerte
    );
}
