using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Th11s.TimeKeeping.Data.Entities
{
    [PrimaryKey(nameof(ArbeitnehmerId), nameof(AbteilungsId), nameof(Datum))]
    public class Tagesdienstzeit : IPersistCalculatedFields
    {
        public Tagesdienstzeit(
            string arbeitnehmerId,
            string abteilungsId,
            DateOnly datum,
            TimeSpan sollarbeitszeit)
        {
            ArbeitnehmerId = arbeitnehmerId;
            AbteilungsId = abteilungsId;
            Datum = datum;
            Sollarbeitszeit = sollarbeitszeit;
        }


        [ForeignKey(nameof(ArbeitnehmerId))]
        public Arbeitnehmer Arbeitnehmer { get; set; }
        public string ArbeitnehmerId { get; set; }


        [ForeignKey(nameof(AbteilungsId))]
        public Abteilung Abteilung { get; set; }
        public string AbteilungsId { get; set; }


        public DateOnly Datum { get; set; }


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

        public TimeSpan Sollarbeitszeit { get; set; }
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
