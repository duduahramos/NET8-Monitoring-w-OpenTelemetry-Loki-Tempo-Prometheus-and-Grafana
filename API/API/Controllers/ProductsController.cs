using CentralizedLoggingAndTracingAPI.Services.ProductService;
using CentralizedLoggingAndTracingAPI.Services.ProductService.DTOs;
using Core.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace CentralizedLoggingAndTracingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient client;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger, IHttpClientFactory clientFactory)
        {
            _productService = productService;
            _logger = logger;
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient("MyApiClient2");
        }

        [HttpGet]
        public async Task<Dictionary<string, object>> Get()
        {
            var list = _productService.GetAllProducts();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "Test");

            var response = await client.SendAsync(requestMessage);
            var result = await response.Content.ReadAsStringAsync();

            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Products", list);
            dictionary.Add("ExternalApiResponse", result);

            _logger.LogInformation($"Get all data from api: {list}");
            return dictionary;
        }

        [HttpPost]
        public IActionResult Post(CreateProductRequest request)
        {
            var result = _productService.CreateProduct(request);

            if (result)
            {
                return Ok(_productService.GetAllProducts);
            }

            return BadRequest($"Product '{request.Name}' failed to create.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _productService.DeleteProduct(id);
            return Ok(result);
        }

    }
}
