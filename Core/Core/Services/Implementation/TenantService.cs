using Microsoft.AspNetCore.Http;
using Core.Services.Interfaces;
using Core.UOW;
using Core.Data;

namespace Core.Services.Implementation
{
    public class TenantService : ITenantService
    {
        private readonly IUnitOfWork _uow;

        public TenantService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public  List<Tenant> GetTenants()
        {
            return _uow.Repository<Tenant>()
                .GetAllOrdered(x => x.Name != "Default", x => x.Id);
        }

        public  bool IsSelectedTenant(string tenantIdFromCookie)
        {
            if (string.IsNullOrEmpty(tenantIdFromCookie))
            {
                return false;
            }

            var tenantId = int.Parse(tenantIdFromCookie);
            return _uow.Repository<Tenant>().GetById(tenantId) is not null;
        }

        public  void SetTenant(HttpContext httpContext)
        {
            //var tenantId = httpContext.Session.GetString("TenantId"); // when application will restart, then it'll be null.
            var tenantId = httpContext.Request.Cookies["TenantId"] ?? 1.ToString(); // setting default tenant if tenant is already not present in cookie.

            if (!string.IsNullOrEmpty(tenantId))
            {
                var tenant = _uow.Repository<Tenant>().GetById<string>(tenantId);
                if (tenant != null)
                {
                    httpContext.Items["TenantId"] = tenant.Id;
                }
                else
                {
                    throw new Exception("Invalid tenant");
                }
            }
            else
            {
                httpContext.Items["TenantId"] = 1.ToString(); // setting default tenant if no tenant is selected.
            }
        }

        public void SetTenant(HttpContext httpContext, string tenantId)
        {

            if (!string.IsNullOrEmpty(tenantId))
            {
                var tenant = _uow.Repository<Tenant>().GetById<string>(tenantId);
                if (tenant != null)
                {
                    httpContext.Items["TenantId"] = tenant.Id;
                }
                else
                {
                    throw new Exception("Invalid tenant");
                }
            }
            else
            {
                httpContext.Items["TenantId"] = 1.ToString(); // setting default tenant if no tenant is selected.
            }
        }
    }
}
