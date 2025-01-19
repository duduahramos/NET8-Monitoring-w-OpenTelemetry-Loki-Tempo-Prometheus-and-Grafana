using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost]
        public IActionResult ChangeTenant(int tenantId)
        {
            //HttpContext.Session.SetString("TenantId", tenantId.ToString());
            HttpContext.Response.Cookies.Append("TenantId", tenantId.ToString(), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                HttpOnly = true,
                IsEssential = true
            });

            _tenantService.SetTenant(HttpContext, tenantId.ToString());

            return Ok();
        }
    }
}
