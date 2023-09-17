using System.ComponentModel.DataAnnotations.Schema;

namespace Th11s.TimeKeeping.Data.Entities
{
    public class Zeiterfassung : IHasUuid
    {
        public Guid Uuid { get; set; }

        [ForeignKey(nameof(ArbeitnehmerId))]
        public Arbeitnehmer? Arbeitnehmer { get; set; }
        public int ArbeitnehmerId { get; set; }

        [ForeignKey(nameof(AbteilungsId))]
        public Abteilung? Abteilung { get; set; }
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
        public bool HatNachbuchungen { get; set; }

        /// <summary>
        /// True, wenn der Eintrag durch einen Benutzer gelöscht wurde.
        /// Wird nur benötigt, um die Nachverfolgung korrekt aufrecht zu erhalten.
        /// Weitere Änderungen am Eintrag sind dadruch verhindert
        /// </summary>
        public bool IstEntfernt { get; set; }
    }
}
