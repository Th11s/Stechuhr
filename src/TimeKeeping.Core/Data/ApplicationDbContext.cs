using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Arbeitnehmer> Arbeitnehmer { get; set; }
        public DbSet<Abteilung> Abteilung { get; set; }

        public DbSet<Stechzeit> Stechzeiten { get; set; }
        public DbSet<Zeiterfassung> Zeiterfassung { get; set; }
        public DbSet<Tagesdienstzeit> Tagesdienstzeiten { get; set; }
    }

    public interface IBerechneteFeldPersistenz
    {
        void BerechneFelder();
    }
}