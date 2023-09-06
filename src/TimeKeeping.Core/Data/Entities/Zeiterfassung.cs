namespace Th11s.TimeKeeping.Data.Entities
{
    public class Zeiterfassung
    {
        public Guid Uuid { get; set; }

        public int ArbeitnehmerId { get; set; }
        public int AbteilungsId { get; set; }


        public required Stechzeit Stechzeit { get; set; }

        /// <summary>
        /// Enthält den gesamten Audit zum Eintrag.
        /// Die Audits werden per JSON gespeichert.
        /// </summary>
        public Nachverfolgungseintrag[]? Nachverfolgung { get; set; }

        /// <summary>
        /// True, wenn der Eintrag verändert, oder retrospektiv gespeichert wurde.
        /// </summary>
        public bool WurdeNachgebucht { get; set; }

        /// <summary>
        /// True, wenn der Eintrag durch einen Benutzer gelöscht wurde.
        /// Wird nur benötigt, um die Nachverfolgung korrekt aufrecht zu erhalten.
        /// Weitere Änderungen am Eintrag sind dadruch verhindert
        /// </summary>
        public bool WurdeEntfernt { get; set; }
    }
}
