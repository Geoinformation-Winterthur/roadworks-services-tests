using Xunit;
using roadwork_portal_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using roadwork_portal_service.Model;
using System.Reflection;

namespace roadworks_portal_service_tests.Controllers;

public class LoginControllerTests
{
    [Fact]
    public void ShouldNotLoginWhenUserIsEmpty()
    {
        Mock loggerMock = new Mock<ILogger<LoginController>>();
        ILogger<LoginController> mockedLogger = (ILogger<LoginController>)loggerMock.Object;

        LoginController loginController = new(mockedLogger);
        IActionResult getResult = loginController.Login(null, true);
        // result has to be a "bad request" since the
        // user is null:
        Assert.IsType<BadRequestObjectResult>(getResult);

        Assert.False(HasResultSecurityTokenString(getResult));

        // now create add an empty user object:
        User loginUser = new();

        getResult = loginController.Login(loginUser, true);
        // result has also to be a "bad request" since the
        // user is empty:
        Assert.IsType<BadRequestObjectResult>(getResult);

        Assert.False(HasResultSecurityTokenString(getResult));

    }

    [Fact]
    public void ShouldNotLoginWhenUserHasNoMailAddress()
    {
        User loginUser = new()
        {
            mailAddress = null
        };

        Mock loggerMock = new Mock<ILogger<LoginController>>();
        ILogger<LoginController> mockedLogger = (ILogger<LoginController>)loggerMock.Object;

        LoginController loginController = new(mockedLogger);
        IActionResult getResult = loginController.Login(loginUser, true);

        Assert.NotNull(getResult);

        // result has to be a "bad request" since the
        // e-mail address is missing:
        Assert.IsType<BadRequestObjectResult>(getResult);

        Assert.False(HasResultSecurityTokenString(getResult));

        // now add an empty e-mail address:
        loginUser.mailAddress = " ";
        getResult = loginController.Login(loginUser, true);

        Assert.NotNull(getResult);

        // result has also to be a "bad request" since the
        // e-mail address is empty:
        Assert.IsType<BadRequestObjectResult>(getResult);

        Assert.False(HasResultSecurityTokenString(getResult));

    }

    [Fact]
    public void ShouldNotLoginWhenUserHasNoPassphrase()
    {
        User loginUser = new()
        {
            mailAddress = "test@test.ch",
            passPhrase = null
        };

        Mock loggerMock = new Mock<ILogger<LoginController>>();
        ILogger<LoginController> mockedLogger = (ILogger<LoginController>)loggerMock.Object;

        LoginController loginController = new(mockedLogger);
        IActionResult getResult = loginController.Login(loginUser, true);

        Assert.NotNull(getResult);

        // result has to be a "bad request" since the
        // passphrase is missing:
        Assert.IsType<BadRequestObjectResult>(getResult);

        Assert.False(HasResultSecurityTokenString(getResult));

        // now add an empty passphrase:
        loginUser.passPhrase = " ";
        getResult = loginController.Login(loginUser, true);

        Assert.NotNull(getResult);

        // result has also to be a "bad request" since the
        // passphrase is empty:
        Assert.IsType<BadRequestObjectResult>(getResult);

        Assert.False(HasResultSecurityTokenString(getResult));

    }

    [Fact]
    public void ShouldNotLoginWithRandomUser()
    {
        User loginUser = new()
        {
            mailAddress = "test@test.ch",
            passPhrase = "test"
        };

        Mock loggerMock = new Mock<ILogger<LoginController>>();
        ILogger<LoginController> mockedLogger = (ILogger<LoginController>)loggerMock.Object;

        LoginController loginController = new(mockedLogger);
        IActionResult getResult = loginController.Login(loginUser, true);

        Assert.NotNull(getResult);

        // result has to be a "unauthorized request" on
        // random user:
        Assert.IsType<UnauthorizedObjectResult>(getResult);

        Assert.False(HasResultSecurityTokenString(getResult));
        
    }

    private static bool HasResultSecurityTokenString(IActionResult getResult)
    {
        PropertyInfo? valuePropertyInfo = getResult.GetType().GetProperty("Value");
        if(valuePropertyInfo != null)
        {
            object? valueObject = valuePropertyInfo.GetValue(getResult, null);
            if(valueObject != null)
            {
                PropertyInfo? securityTokenPropertyInfo = valueObject.GetType().GetProperty("securityTokenString");
                if(securityTokenPropertyInfo != null)
                {
                    object? securityToken = securityTokenPropertyInfo.GetValue(valueObject, null);
                    if(securityToken != null)
                    {
                        string securityTokenString = (string)securityToken;
                        return securityTokenString.Length != 0;
                    }
                }
            }
        }
        return false;
    }

}
