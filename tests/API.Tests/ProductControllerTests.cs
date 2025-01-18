using CentralizedLoggingAndTracingAPI.Controllers;
using CentralizedLoggingAndTracingAPI.Services.ProductService;
using CentralizedLoggingAndTracingAPI.Services.ProductService.DTOs;
using Core.Data;
using Microsoft.AspNetCore.Http;
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
            mockedHttpClient.BaseAddress = new Uri("http://localhost:8000/api/");
            //_httpClientFactory = Mock.Of<IHttpClientFactory>(); // mocking httpclientfactory this will cause to return createClient() always null, so doing like following way to do .Setup() method call on it and returns mocked HttpClient from it.
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockedHttpClient);
            _productService = new();
            _productsController = new ProductsController(_productService.Object, _logger, _httpClientFactory.Object);
        }

        [TestMethod]
        public async Task Get_Method_Should_Return_List_Of_Products()
        {
            var actualProducts = new List<Product>();
            _productService.Setup(x => x.GetAllProducts()).Returns(actualProducts).Verifiable();
            var result = await _productsController.Get();
            var products = (List<Product>)result.FirstOrDefault(x => x.Key == "Products").Value;
            var response = result.FirstOrDefault(x => x.Key == "ExternalApiResponse").Value;

            Assert.AreEqual(products, actualProducts);
            Assert.AreEqual(response, "Successful Request");
        }

        [TestMethod]
        [DataRow("Iphone 20 pro max", "I have launched my new iphone!")]
        public void Post_Method_Should_Create_Product_Successfully(string name, string description)
        {
            CreateProductRequest request = new()
            {
                Name = name,
                Description = description
            };

            _productService.Setup(x => x.CreateProduct(It.IsAny<CreateProductRequest>())).Returns(true).Verifiable();
            var result = (ObjectResult)_productsController.Post(request);

            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
        }
    }
}