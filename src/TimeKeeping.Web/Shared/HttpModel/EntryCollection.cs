using System.ComponentModel.DataAnnotations;

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
            if (entry.Type == EntryType.WorkStart)
            {
                workCount++;
            }

            if (entry.Type == EntryType.WorkEnd)
            {
                workCount--;
            }

            if (entry.Type == EntryType.BreakStart)
            {
                pauseCount++;
            }

            if (entry.Type == EntryType.BreakEnd)
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
