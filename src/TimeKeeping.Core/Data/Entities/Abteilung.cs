namespace Th11s.TimeKeeping.Data.Entities
{
    public class Abteilung
    {
        public Abteilung(string displayName)
        {
            DisplayName = displayName;
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        
        public string DisplayName { get; set; }

        public string? ExternalId { get; set; }
    }
}
