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
        private ILogger<ProductsController> _logger;
        private Mock<IHttpClientFactory> _httpClientFactory;
        private Mock<IProductService> _productService;
        private ProductsController _productsController;

        [TestInitialize]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<ProductsController>>(); // Mock.Of() returns mock.Object so use it only there where you just need mock.Object
                                                              // otherwise if you want to use different operations on it, e.g. changing its method(s)
                                                              // behaviour then you must got for Mock<T> so you'll able to do .Setup() and do other
                                                              // desirable operations on it.

            var mockedHttpClient = Mock.Of<HttpClient>();
            mockedHttpClient.BaseAddress = new Uri("http://api:5000/api/");
            //_httpClientFactory = Mock.Of<IHttpClientFactory>(); // mocking httpclientfactory this will cause to return createClient() always null, so doing like following way to do .Setup() method call on it and returns mocked HttpClient from it.
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockedHttpClient);
            _productService = new();
            _productsController = new ProductsController(_productService.Object, _logger, _httpClientFactory.Object);
        }

        [TestMethod]
        public async Task Get_Products_Should_Returns_List_Of_Products()
        {
            var result = await _productsController.Get();

            var expected = _productService.Setup(x => x.GetAllProducts()).Returns(() => new List<Product>());

            Assert.IsInstanceOfType<IActionResult>(result);
        }

        //private IEnumerable<Product> Products()
        //{
        //    return new List<Product>();
        //}
    }
}