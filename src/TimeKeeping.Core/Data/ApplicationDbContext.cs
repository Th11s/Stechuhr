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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Zeiterfassung>(e =>
            {
                e.ComplexProperty(p => p.Stechzeit);
                e.HasIndex(p => p.Stechzeit.Datum);
            });
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            VerarbeiteFehlendeUuid();
            VerarbeiteBerechneteFelder();

            return base.SaveChangesAsync(cancellationToken);
        }


        public void VerarbeiteFehlendeUuid()
        {
            var entries = ChangeTracker.Entries()
               .Where(x => EntityState.Added == x.State)
               .Select(x => x.Entity)
               .OfType<IHasUuid>()
               .Where(x => x.Uuid == default);

            foreach (var e in entries)
                e.Uuid = Guid.NewGuid();
        }


        public void VerarbeiteBerechneteFelder()
        {
            var entries = ChangeTracker.Entries()
                .Where(x => new[] { EntityState.Added, EntityState.Modified }.Contains(x.State))
                .Select(x => x.Entity)
                .OfType<IPersistCalculatedFields>();

            foreach (var e in entries)
                e.CalculateFields();
        }
    }
}