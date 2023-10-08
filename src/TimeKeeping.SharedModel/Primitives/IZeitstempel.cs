namespace Th11s.TimeKeeping.SharedModel.Primitives
{
    public interface IZeitstempel
    {
        DateTimeOffset Zeitstempel { get; }
        Stempeltyp Stempeltyp { get; }


        bool IstArbeitsanfang() => Stempeltyp == Stempeltyp.Arbeitsanfang;
        bool IstArbeitsende() => Stempeltyp == Stempeltyp.Arbeitsende;
        bool IstPausenanfang() => Stempeltyp == Stempeltyp.Pausenanfang;
        bool IstPausenende() => Stempeltyp == Stempeltyp.Pausenende;
    }
}
