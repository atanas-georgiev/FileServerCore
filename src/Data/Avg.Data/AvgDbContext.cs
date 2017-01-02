namespace Avg.Data
{
    using System;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Avg.Data.Common.Models;
    using Avg.Data.Models;

    public class AvgDbContext : IdentityDbContext<AvgUser>
    {
        public AvgDbContext(DbContextOptions options)
            : base(options)
        {
        }

//        public virtual DbSet<Album> Albums { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Album>().HasOne(p => p.Cover).WithMany(b => b.Covers);
            //modelBuilder.Entity<Image>().HasMany(x => x.Covers).WithOne(x => x.Cover).HasForeignKey(x => x.CoverId);
            base.OnModelCreating(modelBuilder);
        }

        private void ApplyAuditInfoRules()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditInfo
                    && ((entry.State == EntityState.Added) || (entry.State == EntityState.Modified)))
                {
                    var entity = entry.Entity as IAuditInfo;
                    if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                    else
                    {
                        entity.ModifiedOn = DateTime.Now;
                    }
                }
            }
        }
    }
}