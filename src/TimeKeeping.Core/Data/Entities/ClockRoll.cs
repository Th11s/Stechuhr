namespace Th11s.TimeKeeping.Data.Entities
{
    public class RollingClockEntry
    {
        public Guid Uuid { get; set; }

        public int UserId { get; set; }
        public int OrgUnitId { get; set; }


        public required Timestamp Stamp { get; set; }
        public AggregationInfo? Aggregation { get; set; }


        /// <summary>
        /// Enthält den gesamten Audit zum Eintrag.
        /// Die Audits werden per JSON gespeichert, um später den Übergang in die Tages-, Wochen-, Monats- und Jahresaggregate zu vereinfachen.
        /// </summary>
        public AuditCollection ChangeTracking { get; set; } = new();

        /// <summary>
        /// True, wenn der Eintrag verändert, oder retrospektiv gespeichert wurde.
        /// </summary>
        public bool HasBeenModified { get; set; }

        /// <summary>
        /// True, wenn der Eintrag durch einen Benutzer gelöscht wurde.
        /// Wird nur benötigt, um den Audit korrekt aufrecht zu erhalten.
        /// ChangeTracking enthält einen entsprechenden Eintrag.
        /// Weitere Änderungen am Eintrag sind dadruch verhindert
        /// </summary>
        public bool HasBeenDeleted { get; set; }
    }
}
