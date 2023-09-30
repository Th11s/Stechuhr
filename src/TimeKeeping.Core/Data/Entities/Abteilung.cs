using System.ComponentModel.DataAnnotations.Schema;

namespace Th11s.TimeKeeping.Data.Entities
{
    //TODO AP: string id, damit die ID im API-Link verwertet werden kann?
    public class Abteilung(string id, string displayName, string? parentId)
    {
        public string Id { get; set; } = id;

        [ForeignKey(nameof(ParentId))]
        public Abteilung? Parent { get; set; }
        public string? ParentId { get; set; } = parentId;

        public string DisplayName { get; set; } = displayName;

        public ICollection<Arbeitnehmer>? Arbeitnehmer { get; set; }


        public string? ExternalId { get; set; }
    }
}
