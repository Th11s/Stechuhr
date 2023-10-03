using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Th11s.TimeKeeping.Data.Entities
{
    [PrimaryKey(nameof(ArbeitsplatzId), nameof(Datum))]
    public class Tagesdienstzeit : IPersistCalculatedFields
    {
        public Tagesdienstzeit(
            Guid arbeitsplatzId,
            DateOnly datum)
        {
            ArbeitsplatzId = arbeitsplatzId;
            Datum = datum;
        }


        [ForeignKey(nameof(ArbeitsplatzId))]
        public Arbeitsplatz? Arbeitsplatz { get; set; }
        public Guid ArbeitsplatzId { get; set; }


        public DateOnly Datum { get; set; }

        [ConcurrencyCheck]
        public DateTimeOffset LastModified { get; set; }


        /// <summary>
        /// Enthält die Arbeitszeit für den Tag
        /// </summary>
        public TimeSpan? Arbeitszeit { get; set; }

        /// <summary>
        /// Enthält die Gesamtpausenzeit für den Tag.
        /// </summary>
        public TimeSpan? Pausenzeit { get; set; }
        public bool HatPausezeitminimum { get; set; }

        public required TimeSpan Sollarbeitszeit { get; set; }
        public TimeSpan? Arbeitszeitgutschrift { get; set; }

        public TimeSpan Zeitsaldo { get; private set; } = TimeSpan.Zero;


        public string[] Probleme { get; set; } = Array.Empty<string>();
        public bool HatProbleme { get; set; }


        public void CalculateFields()
        {
            Zeitsaldo = (Arbeitszeit ?? TimeSpan.Zero)
                + (Arbeitszeitgutschrift ?? TimeSpan.Zero) 
                - Sollarbeitszeit;

            HatProbleme = Probleme.Any();
        }
    }
}
