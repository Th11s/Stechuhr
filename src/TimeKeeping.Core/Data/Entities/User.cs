using Microsoft.AspNetCore.Identity;

namespace Th11s.TimeKeeping.Data.Entities
{
    public class User : IdentityUser
    {
        public string Anzeigename { get; set; }

        public ICollection<Arbeitsplatz>? Arbeitsplaetze { get; set; }

        public string? ExternalId { get; set; }
    }
}
