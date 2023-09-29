using System.ComponentModel.DataAnnotations;
using Th11s.TimeKeeping.SharedModel.Primitives;

namespace TimeKeeping.Web.Shared.HttpModel;

public class EntryCollection : IValidatableObject
{
    public List<Entry> Entries { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {

        var pauseCount = 0;
        var workCount = 0;
        foreach (var entry in Entries.OrderBy(x => x.At))
        {
            if (entry.Type == Stempeltyp.Arbeitsanfang)
            {
                workCount++;
            }

            if (entry.Type == Stempeltyp.Arbeitsende)
            {
                workCount--;
            }

            if (entry.Type == Stempeltyp.Pausenanfang)
            {
                pauseCount++;
            }

            if (entry.Type == Stempeltyp.Pausenende)
            {
                pauseCount--;
            }

            if (pauseCount < 0 || workCount < 0)
            {
                yield return new("invalid chronology");
            }
        }


        if (pauseCount != 0 || workCount != 0)
        {
            yield return new("missing entries");
        }
    }
}
