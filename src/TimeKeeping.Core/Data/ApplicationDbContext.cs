using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Arbeitnehmer> Arbeitnehmer { get; set; }
        public DbSet<Abteilung> Abteilung { get; set; }


        public DbSet<Zeiterfassung> Zeiterfassung { get; set; }
        public DbSet<Tagesdienstzeit> Tagesdienstzeiten { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            VerarbeiteBerechneteFelder();

            return base.SaveChangesAsync(cancellationToken);
        }

        public void VerarbeiteBerechneteFelder()
        {
            var entries = ChangeTracker.Entries()
                .Where(x => new[] { EntityState.Added, EntityState.Modified }.Contains(x.State))
                .Select(x => x.Entity)
                .OfType<IBerechneteFeldPersistenz>();

            foreach (var e in entries)
                e.BerechneFelder();
        }
    }
}