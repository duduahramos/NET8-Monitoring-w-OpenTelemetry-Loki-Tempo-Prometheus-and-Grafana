using CentralizedLoggingAndTracingAPI.Controllers;
using CentralizedLoggingAndTracingAPI.Services.ProductService;
using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Tests
{
    [TestClass]
    public class ProductControllerTests
    {
        private readonly ProductsController _productsController;
        private Mock<IProductService> productService = new Mock<IProductService>();
        private Mock<ILogger<ProductsController>> logger = new Mock<ILogger<ProductsController>>();
        private Mock<IHttpClientFactory> httpClientFactory = new Mock<IHttpClientFactory>();

        public ProductControllerTests()
        {   
            _productsController = new ProductsController(productService.Object, logger.Object, httpClientFactory.Object);
        }

        [TestMethod]
        public async void Get_Products()
        {
            var result = await _productsController.Get();

            var expected = productService.Setup(x => x.GetAllProducts()).Returns(Products());

            Assert.IsInstanceOfType<IActionResult>(result);
        }

        private IEnumerable<Product> Products()
        {
            return new List<Product>();
        }
    }
}