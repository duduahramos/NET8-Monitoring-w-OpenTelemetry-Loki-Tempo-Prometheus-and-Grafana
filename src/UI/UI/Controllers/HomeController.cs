using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Core.Data;
using UI.Controllers.Base;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient client;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("MyApiClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tenantId = HttpContext.Request.Cookies["TenantId"] ?? HttpContext.Items["TenantId"].ToString();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "Products");
            requestMessage.Headers.Add("TenantId", tenantId);

            // Send the request
            var response = await client.SendAsync(requestMessage);

            // Ensure the response is successful
            if (!response.IsSuccessStatusCode)
            {
                // Handle error
                return StatusCode((int)response.StatusCode, "An error occurred while retrieving products.");
            }

            // Read and process the response content
            var content = await response.Content.ReadAsStringAsync();
            ViewData["Message"] = content;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string productName)
        {
            var product = new Product()
            {
                Name = productName,
                Description = $"This is {productName}"
            };

            var tenantId = HttpContext.Request.Cookies["TenantId"] ?? HttpContext.Items["TenantId"].ToString();

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Products")
            {
                Content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(tenantId))
            {
                requestMessage.Headers.Add("TenantId", tenantId);
            }

            var response = await client.SendAsync(requestMessage);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
