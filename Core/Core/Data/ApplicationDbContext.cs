using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentTenantService _currentTenantService;
        public string CurrentTenantId { get; set; }


        // Constructor -- convention used by Entity Framework 
        public ApplicationDbContext(ICurrentTenantService currentTenantService, DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _currentTenantService = currentTenantService;
            CurrentTenantId = _currentTenantService.TenantId;

        }

        // DbSets -- create for all entity types to be managed with EF
        public DbSet<Product> Products { get; set; }
        public DbSet<Tenant> Tenants { get; set; } // To create migration only, first comment this line.

        public override int SaveChanges()
        {

            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList()) // Write tenant Id to table
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = CurrentTenantId;
                        break;
                }
            }


            var result = base.SaveChanges();
            return result;
        }


    }
}
