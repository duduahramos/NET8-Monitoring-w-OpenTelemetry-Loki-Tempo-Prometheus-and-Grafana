using Core.Data;
using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.Services.Implementation
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly TenantDbContext _context;

        public CurrentTenantService(TenantDbContext context)
        {
            _context = context;

        }
        public async Task<bool> SetTenant(string tenant)
        {

            var tenantInfo = await _context.Tenants.Where(x => x.Id == tenant).FirstOrDefaultAsync();
            if (tenantInfo != null)
            {
                TenantId = tenant;
                return true;
            }
            else
            {
                throw new Exception("Tenant invalid");
            }

        }
        public string? TenantId { get; set; }
    }
}
