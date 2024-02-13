using FluentAssertions;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Integration.Tests.Helper;
using Kanban.Model.Dto.Repository.Card;
using Newtonsoft.Json;
using System.Text;

namespace Kanban.Integration.Tests.Tests;

public class AuthIntegrationTests : IntegrationTestsSetup
{
    public AuthIntegrationTests(ApiWebApplicationFactory fixture) : base(fixture)
    {
    }

    [Theory]
    [MemberData(nameof(GetUnauthorizeParameters))]
    public async Task AllCardEndpoints_ShouldReturnUnauthorized_WhenTokenIsNotProvided(string method, string url)
    {
        // Arrange
        var payload = JsonConvert.DeserializeObject<CardDto>(Mocks.InsertMockObject);
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = method switch
        {
            "GET" => await _client.GetAsync(url),
            "POST" => await _client.PostAsync(url, jsonContent),
            "DELETE" => await _client.DeleteAsync(url),
            "PUT" => await _client.PutAsync(url, jsonContent),
            _ => await _client.GetAsync("Cards"),
        };

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Auth_ShouldAllowRequest_WhenValidCredentialsAreSent()
    {
        // Arrange
        var client = new Model.Dto.API.Auth.ClientDto
        {
            Id = "client",
            Secret = "secret"
        };
        using StringContent jsonContent = new(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");

        // Act
        AuthenticationHelper.SetupAuthenticationHeader(_client, $"{client.Id}:{client.Secret}");
        var getResponse = await _client.GetAsync("Cards");

        // Assert
        getResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Register_ShouldRegisterNewClient_WhenValidCredentialsAreSent()
    {
        // Arrange
        var client = new Model.Dto.API.Auth.ClientDto
        {
            Id = "new-client",
            Secret = "secret"
        };
        using StringContent jsonContent = new(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");


        // Act
        var registerResponse = await _client.PostAsync("auth/register", jsonContent);
        AuthenticationHelper.SetupAuthenticationHeader(_client, $"{client.Id}:{client.Secret}");
        var getResponse = await _client.GetAsync("Cards");

        // Assert
        registerResponse.IsSuccessStatusCode.Should().BeTrue();
        getResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    private static IEnumerable<object[]> GetUnauthorizeParameters()
    {
        return new List<object[]>
        {
            new object[] { "GET", "Cards" },
            new object[] { "POST","Cards" },
            new object[] { "DELETE", "Cards/65c6e255a03db52a8056230f" },
            new object[] { "PUT", "Cards/65c6e255a03db52a8056230f" },
        };
    }
}
