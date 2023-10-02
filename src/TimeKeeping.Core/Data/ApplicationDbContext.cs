using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Arbeitsplatz> Arbeitsplaetze { get; set; }
        public DbSet<Abteilung> Abteilung { get; set; }


        public DbSet<Zeiterfassung> Zeiterfassung { get; set; }
        public DbSet<Tagesdienstzeit> Tagesdienstzeiten { get; set; }



        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CreateMissingUuids();
            CalculateMaterializedFields();

            return base.SaveChangesAsync(cancellationToken);
        }


        public void CreateMissingUuids()
        {
            var entries = ChangeTracker.Entries()
               .Where(x => EntityState.Added == x.State)
               .Select(x => x.Entity)
               .OfType<IHasUuid>()
               .Where(x => x.Id == default);

            foreach (var e in entries)
                e.Id = Guid.NewGuid();
        }


        public void CalculateMaterializedFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(x => new[] { EntityState.Added, EntityState.Modified }.Contains(x.State))
                .Select(x => x.Entity)
                .OfType<IPersistCalculatedFields>();

            foreach (var e in entries)
                e.CalculateFields();
        }
    }

    public class NpgsqlDbContext : ApplicationDbContext
    {
        public NpgsqlDbContext(DbContextOptions<NpgsqlDbContext> options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Zeiterfassung>(
                e =>
                {
                    e.Property(p => p.Nachverfolgung)
                        .HasColumnType("jsonb");
                });
        }
    }

    public class SqlServerDbContext : ApplicationDbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Zeiterfassung>(
                e =>
                {
                    e.OwnsMany(p => p.Nachverfolgung)
                        .ToJson();
                });
        }
    }
}