using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class TenantDbContext : DbContext
    {
        // This context is for looking up the tenant when a request comes in.
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Tenant>()
                .HasData(
                    new Tenant()
                    {
                        Id = 1.ToString(),
                        Name = "Default"
                    },
                    new Tenant()
                    {
                        Id = 2.ToString(),
                        Name = "Tenant 1"
                    },
                    new Tenant()
                    {
                        Id = 3.ToString(),
                        Name = "Tenant 2"
                    }
                );
        }
    }
}
