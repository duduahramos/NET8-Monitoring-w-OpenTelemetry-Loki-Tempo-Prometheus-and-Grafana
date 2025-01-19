using Core.Data;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Interfaces
{
    public interface ITenantService
    {
        void SetTenant(HttpContext context);
        void SetTenant(HttpContext httpContext, string tenantId);
        List<Tenant> GetTenants();
        bool IsSelectedTenant(string tenantIdFromCookie);
    }
}