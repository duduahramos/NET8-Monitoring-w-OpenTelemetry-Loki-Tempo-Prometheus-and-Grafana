using API2.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Tests
{
    [TestClass]
    public class TestControllerTests
    {
        private readonly TestController _testController = new();

        [TestMethod]
        public void Get_Method_Should_Return_Response()
        {
            var expectedResponse = "Successful Request";

            var result = (ObjectResult)_testController.Get();

            Assert.IsInstanceOfType<IActionResult>(result);
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            Assert.AreEqual(result.Value, expectedResponse);
        }
    }
}