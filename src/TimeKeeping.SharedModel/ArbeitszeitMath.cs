using Th11s.TimeKeeping.SharedModel.Primitives;

namespace Th11s.TimeKeeping.SharedModel
{
    public static class ArbeitszeitMath
    {
        public static List<(TZeitstempel Anfang, TZeitstempel Ende)> PaareUndValidiereZeitstempel<TZeitstempel>(IEnumerable<TZeitstempel> stechzeiten, List<string> probleme)
            where TZeitstempel : IZeitstempel
        {
            var zeitpaare = new List<(TZeitstempel Anfang, TZeitstempel Ende)>();
            var stack = new Stack<TZeitstempel>();
            foreach (var st in stechzeiten)
            {
                if (stack.Count == 0)
                {
                    if (st.IstArbeitsanfang())
                    {
                        stack.Push(st);
                    }
                    else
                    {
                        probleme.Add("Zeitberechnung:Arbeitsbeginn");
                    }
                }

                else
                {
                    if (st.IstPausenanfang() && stack.Peek().IstArbeitsanfang())
                    {
                        stack.Push(st);
                    }
                    else
                    {
                        probleme.Add("Zeitberechnung:Pausenanfang");
                    }

                    if (st.IstPausenende() && stack.Peek().IstPausenanfang())
                    {
                        zeitpaare.Add((stack.Pop(), st));
                    }
                    else
                    {
                        probleme.Add("Zeitberechnung:Pausenende");
                    }

                    if (st.IstArbeitsende() && stack.Peek().IstArbeitsanfang())
                    {
                        zeitpaare.Add((stack.Pop(), st));
                    }
                    else
                    {
                        probleme.Add("Zeitberechnung:Arbeitsende");
                    }
                }
            }

            return zeitpaare;
        }

        public static (TimeSpan Arbeitszeit, TimeSpan Pausenzeit) BerechneArbeitsUndPausenzeit<TZeitstempel>(List<(TZeitstempel Anfang, TZeitstempel Ende)> zeitpaare)
            where TZeitstempel : IZeitstempel
        {
            var arbeitszeit = TimeSpan.Zero;
            var pausenzeit = TimeSpan.Zero;

            foreach (var (anfang, ende) in zeitpaare)
            {
                var dauer = ende.Zeitstempel - anfang.Zeitstempel;

                if (anfang.IstArbeitsanfang())
                    arbeitszeit += dauer;

                if (anfang.IstPausenanfang())
                    pausenzeit += dauer;
            }

            return (arbeitszeit, pausenzeit);
        }
    }
}
