using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Th11s.TimeKeeping.Data.Entities
{
    public class Arbeitsplatz : IHasUuid
    {
        public Arbeitsplatz(string arbeitnehmerId, int abteilungsId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(arbeitnehmerId);

            ArbeitnehmerId = arbeitnehmerId;
            AbteilungsId = abteilungsId;
        }

        [Key]
        public Guid Id { get; set; }


        [ForeignKey(nameof(ArbeitnehmerId))]
        public User? Arbeitnehmer { get; set; }
        public string ArbeitnehmerId { get; set; }


        [ForeignKey(nameof(AbteilungsId))]
        public Abteilung? Abteilung { get; set; }
        public int AbteilungsId { get; set; }



        public TimeSpan Standarddienstzeit { get; set; }

        public string? ExternalId { get; set; }
    }
}
