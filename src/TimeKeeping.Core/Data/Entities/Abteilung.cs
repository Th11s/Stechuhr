using System.ComponentModel.DataAnnotations.Schema;

namespace Th11s.TimeKeeping.Data.Entities
{
    public class Abteilung(int id, int? parentId, string displayName)
    {
        public int Id { get; set; } = id;

        [ForeignKey(nameof(ParentId))]
        public Abteilung? Parent { get; set; }
        public int? ParentId { get; set; } = parentId;

        public string DisplayName { get; set; } = displayName;

        public ICollection<Arbeitsplatz>? Arbeitsplaetze { get; set; }


        public string? ExternalId { get; set; }
    }
}
