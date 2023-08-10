using Xunit;
using roadwork_portal_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;

namespace roadworks_portal_service_tests.Controllers;

public class HomeControllerTests
{
    [Fact]
    public void TestCorrectActiveMessage()
    {
        Mock loggerMock = new Mock<ILogger<HomeController>>();
        ILogger<HomeController> mockedLogger = (ILogger<HomeController>)loggerMock.Object;

        HomeController homeController = new HomeController(mockedLogger);
        ActionResult<string> getResult = homeController.Get();
        Assert.Equal("\"roadworks services are working.\"", getResult.Value);
    }
}