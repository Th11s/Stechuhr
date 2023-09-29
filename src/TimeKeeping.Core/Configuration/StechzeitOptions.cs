namespace Th11s.TimeKeeping.Configuration
{
    public class StechzeitOptions
    {
        public TimeSpan Nachbuchungsschwelle { get; set; }

        public TimeSpan MinimaleStandardpause { get; set; } = TimeSpan.FromMinutes(30);
        public List<MinPause> MinimalePausenzeiten { get; set; } = new();
    }

    public class MinPause
    {
        public TimeSpan Arbeitszeit { get; set; }
        public TimeSpan Pause { get; set; }
    }

    public static class StechzeitOptionsExtensions
    {
        public static TimeSpan? GetMinimalePause(this StechzeitOptions options, TimeSpan arbeitszeit)
        {
            var pause = options.MinimalePausenzeiten.OrderBy(x => x.Arbeitszeit)
                .SkipWhile(x => x.Arbeitszeit < arbeitszeit)
                .Select(x => x.Pause)
                .FirstOrDefault();

            return pause == default 
                ? options.MinimaleStandardpause 
                : pause;
        }
    }
}
