﻿using Microsoft.EntityFrameworkCore;

namespace Th11s.TimeKeeping.Data.Entities
{
    [PrimaryKey(nameof(ArbeitnehmerId), nameof(AbteilungsId), nameof(Datum))]
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
