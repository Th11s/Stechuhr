namespace Th11s.TimeKeeping.Data.Entities
{
    public class Tagesdienstzeit : IBerechneteFeldPersistenz
    {
        public Guid Uuid { get; set; }

        public int ArbeitnehmerId { get; set; }
        public int AbteilungsId { get; set; }


        public DateOnly Datum { get; set; }

        /// <summary>
        /// Enthält die geleistete Arbeitszeit für den Tag - Pausenzeiten sind bereits abgezogen.
        /// </summary>
        public TimeSpan Arbeitszeit { get; set; }

        /// <summary>
        /// Enthält die Gesamtpausenzeit für den Tag.
        /// </summary>
        public TimeSpan Pausenzeit { get; set; }

        public TimeSpan Sollarbeitszeit { get; set; }


        public TimeSpan Zeitsaldo { get; set; }

        public void BerechneFelder()
        {
            Zeitsaldo = Arbeitszeit - Sollarbeitszeit;
        }
    }
}
