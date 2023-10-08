using Microsoft.AspNetCore.Identity;

namespace Th11s.TimeKeeping.Data.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Arbeitsplatz>? Arbeitsplaetze { get; set; }

    }
}
