using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Middlewares
{
    public class UiTenantMiddleware : IMiddleware
    {
        private readonly ITenantService _tenantService;

        public UiTenantMiddleware(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _tenantService.SetTenant(context);
            
            await next(context);
        }
    }

}
