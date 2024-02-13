using FluentAssertions;
using Kanban.API.Authentication;
using Kanban.Application.Interfaces;
using Kanban.CrossCutting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using System.Text;
using System.Text.Encodings.Web;
using App = Kanban.Model.Dto.Application.Client;

namespace Kanban.Presentation.Tests;

public class AuthenticationTests
{
    private readonly Mock<IOptionsMonitor<AuthenticationSchemeOptions>> _options;
    private readonly Mock<UrlEncoder> _encoder;
    private readonly Mock<ILoggerFactory> _loggerFactory;
    private readonly Mock<ISystemClock> _clock;
    private readonly Mock<IAuthService> _authService;
    private readonly CustomAuthenticationHandler _handler;

    public AuthenticationTests()
    {
        _options = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();

        // This Setup is required for .NET Core 3.1 onwards.
        _options
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(new AuthenticationSchemeOptions());

        var logger = new Mock<ILogger<CustomAuthenticationHandler>>();
        _loggerFactory = new Mock<ILoggerFactory>();
        _loggerFactory.Setup(x => x.CreateLogger(It.IsAny<String>())).Returns(logger.Object);

        _encoder = new Mock<UrlEncoder>();
        _clock = new Mock<ISystemClock>();
        _authService = new Mock<IAuthService>();

        _handler = new CustomAuthenticationHandler(_options.Object, _loggerFactory.Object, _encoder.Object, _clock.Object, _authService.Object);
    }

    [Theory]
    [InlineData(":secret")]
    [InlineData("client:")]
    [InlineData("clientsecret")]
    public async Task Authorization_ShouldBeDenied_WhenHeaderFormatIsInvalid(string credentials)
    {
        // Arrange
        var context = new DefaultHttpContext();
        string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(credentials));
        var authorizationHeader = new StringValues($"{Constants.Authentication} {svcCredentials}");
        context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);
        
        // Act
        await _handler.InitializeAsync(new AuthenticationScheme(Constants.Authentication, null, typeof(CustomAuthenticationHandler)), context);
        var response = await _handler.AuthenticateAsync();

        // Assert
        response.Succeeded.Should().BeFalse();
        response.Failure.Message.Should().Be(Constants.InvalidAuthorizationHeaderFormat);
    }

    [Theory]
    [InlineData("client:secre")]
    public async Task Authorization_ShouldBeDenied_WhenCredentialsAreInvalid(string credentials)
    {
        // Arrange
        var client = new App.ClientDto
        {
            Id = "client",
            Secret = "secret"
        };
        _authService.Setup(x => x.Login(It.Is<App.ClientDto>
            (x => x.Id == client.Id && x.Secret == client.Secret)))
            .ReturnsAsync(true);

        var context = new DefaultHttpContext();
        string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(credentials));
        var authorizationHeader = new StringValues($"{Constants.Authentication} {svcCredentials}");
        context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);

        // Act
        await _handler.InitializeAsync(new AuthenticationScheme(Constants.Authentication, null, typeof(CustomAuthenticationHandler)), context);
        var response = await _handler.AuthenticateAsync();

        // Assert
        response.Succeeded.Should().BeFalse();
        response.Failure.Message.Should().Be(Constants.InvalidIdOrSecret);
    }

    [Fact]
    public async Task Authorization_ShouldBeDenied_WhenBearerTokenIsProvided()
    {
        // Arrange
        var context = new DefaultHttpContext();
        string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("client:secret"));
        var authorizationHeader = new StringValues($"Bearer {svcCredentials}");
        context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);

        // Act
        await _handler.InitializeAsync(new AuthenticationScheme(Constants.Authentication, null, typeof(CustomAuthenticationHandler)), context);
        var response = await _handler.AuthenticateAsync();

        // Assert
        response.Succeeded.Should().BeFalse();
        response.Failure.Message.Should().Be(Constants.AuthorizationHeaderMalformed);
    }

}