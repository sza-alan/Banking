using Banking.Api.Models;
using Banking.API.Models;
using Banking.API.Models.ReadModels;
using Banking.API.Models.Snapshots;
using Microsoft.EntityFrameworkCore;

namespace Banking.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<StoredEvent> StoredEvents => Set<StoredEvent>();
        public DbSet<AccountReadModel> AccountReadModels => Set<AccountReadModel>();
        public DbSet<AccountSnapshot> AccountSnapshots => Set<AccountSnapshot>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StoredEvent>()
                .HasIndex(x => new
                {
                    x.AggregateId,
                    x.Version
                })
                .IsUnique();

            modelBuilder.Entity<AccountSnapshot>()
                .HasKey(x => x.AggregateId);
        }
    }
}
