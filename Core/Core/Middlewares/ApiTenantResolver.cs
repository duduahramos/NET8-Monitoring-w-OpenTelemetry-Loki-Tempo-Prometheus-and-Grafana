using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Middlewares
{
    public class ApiTenantResolver
    {
        private readonly RequestDelegate _next;
        public ApiTenantResolver(RequestDelegate next)
        {
            _next = next;
        }

        // Get Tenant Id from incoming requests 
        public async Task InvokeAsync(HttpContext context, ICurrentTenantService currentTenantService)
        {
            //var tenant = context.Request.Cookies["TenantId"];
            var tenant = context.Request.Headers["TenantId"];
            if (!string.IsNullOrEmpty(tenant))
            {
                await currentTenantService.SetTenant(tenant);
            }

            await _next(context);
        }


    }
}
