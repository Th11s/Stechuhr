using System.Runtime.Intrinsics.X86;

namespace Th11s.TimeKeeping.Data.Entities
{
    public class Tagesdienstzeit : IPersistCalculatedFields
    {
        public Tagesdienstzeit(
            string arbeitnehmerId,
            int abteilungsId,
            DateOnly datum,
            TimeSpan sollarbeitszeit)
        {
            ArbeitnehmerId = arbeitnehmerId;
            AbteilungsId = abteilungsId;
            Datum = datum;
            Sollarbeitszeit = sollarbeitszeit;
        }

        public string ArbeitnehmerId { get; set; }
        public int AbteilungsId { get; set; }

        public DateOnly Datum { get; set; }


        /// <summary>
        /// Enthält die _geleistete_ Arbeitszeit für den Tag - Pausenzeiten sind bereits abgezogen.
        /// </summary>
        public TimeSpan? Arbeitszeit { get; set; }


        public TimeSpan Sollarbeitszeit { get; set; }
        public TimeSpan? Arbeitszeitgutschrift { get; set; }

        public TimeSpan Zeitsaldo { get; set; } = TimeSpan.Zero;


        /// <summary>
        /// Enthält die Gesamtpausenzeit für den Tag.
        /// </summary>
        public TimeSpan? Pausenzeit { get; set; }


        public string[] Probleme { get; set; } = Array.Empty<string>();
        public bool HatProbleme { get; set; }


        public void CalculateFields()
        {
            Zeitsaldo = (Arbeitszeit ?? TimeSpan.Zero) + (Arbeitszeitgutschrift ?? TimeSpan.Zero) - Sollarbeitszeit;
            HatProbleme = Probleme.Any();
        }
    }
}
