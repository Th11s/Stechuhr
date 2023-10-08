using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Th11s.TimeKeeping.SharedModel.Primitives;

namespace Th11s.TimeKeeping.Data.Entities
{
    [Index(nameof(Datum))]
    public class Zeiterfassung : IHasUuid, IZeitstempel
    {
        public Zeiterfassung(Guid arbeitsplatzId)
        {
            ArbeitsplatzId = arbeitsplatzId;
        }

        [SetsRequiredMembers]
        public Zeiterfassung(Guid arbeitsplatzId, DateOnly datum, DateTimeOffset zeitstempel, Stempeltyp stechTyp) 
            : this(arbeitsplatzId)
        {
            Datum = datum;
            Zeitstempel = zeitstempel;
            Stempeltyp = stechTyp;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTimeOffset LastModified { get; set; }


        [ForeignKey(nameof(ArbeitsplatzId))]
        public Arbeitsplatz? Arbeitsplatz { get; set; }
        public Guid ArbeitsplatzId { get; set; }



        public required DateOnly Datum { get; set; }
        public required DateTimeOffset Zeitstempel { get; set; }
        public required Stempeltyp Stempeltyp { get; set; }

        /// <summary>
        /// Enthält den gesamten Audit zum Eintrag.
        /// Die Audits werden per JSON gespeichert.
        /// </summary>
        public Nachverfolgungseintrag[]? Nachverfolgung { get; set; }


        public bool IstNachbuchung { get; set; }
        public bool IstVorausbuchung { get; set; }


        /// <summary>
        /// Wahr, wenn der Eintrag verändert wurde
        /// </summary>
        public bool HatAnpassungen { get; set; }


        /// <summary>
        /// True, wenn der Eintrag durch einen Benutzer gelöscht wurde.
        /// Wird nur benötigt, um die Nachverfolgung korrekt aufrecht zu erhalten.
        /// Weitere Änderungen am Eintrag sind dadruch verhindert
        /// </summary>
        public bool IstEntfernt { get; set; }
    }
}
