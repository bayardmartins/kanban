using FluentAssertions;
using Kanban.API.Dto.Card;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Integration.Tests.Helper;
using Newtonsoft.Json;
using System.Text;

namespace Kanban.Integration.Tests.Tests;

public class CardsIntegrationTests : IntegrationTestsSetup
{
    private string GetCredentials()
    {
        var client = JsonConvert.DeserializeObject<Repository.Dto.Models.ClientDto>(Mocks.ClientMock);
        return $"{client._id}:{client.Secret}";
    }

    public CardsIntegrationTests(ApiWebApplicationFactory fixture) : base(fixture)
    {
    }

    [Theory]
    [InlineData("Cards")]
    [InlineData("Cards/65c6e255a03db52a8056230f")]
    public async Task GetCards_EndpointsReturnSuccessAndCorrectContent(string url)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = JsonConvert.DeserializeObject<GetCardResponseDto>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
        content.Cards.Count.Should().NotBe(0);
    }

    [Fact]
    public async Task CreateCards_EndpointsReturnSuccessAndCorrectContent()
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = JsonConvert.DeserializeObject<CardDto>(Mocks.InsertMockObject);
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("Cards", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = JsonConvert.DeserializeObject<CreateCardResponseDto>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
        content.CreatedCard.Should().NotBeNull();
        content.CreatedCard.Id.Should().NotBeNullOrEmpty();
        content.CreatedCard.Name.Should().Be(payload.Name);
        content.CreatedCard.Description.Should().Be(payload.Description);
    }

    [Theory]
    [MemberData(nameof(GetDeleteParameters))]
    public async Task DeleteCards_EndpointsReturnSuccessOrFail(string url, bool result)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.DeleteAsync(url);

        // Assert
        response.IsSuccessStatusCode.Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(GetUpdateParameters))]
    public async Task UpdateCards_EndpointsReturnSuccessOrFail(string url, bool result)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockTwo);
        payload.Name = "new name";
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync(url, jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().Be(result);
    }

    private static IEnumerable<object[]> GetDeleteParameters() => new List<object[]>
    {
        new object[] { "Cards/65c6e255a03db52a8056230f", true },
        new object[] { "Cards/65c7c4ea7d5a911ae3d662e4", false },
    };

    private static IEnumerable<object[]> GetUpdateParameters() => new List<object[]>
    {
        new object[] { "Cards/65c77ba67d5a911ae3d662db", true },
        new object[] { "Cards/65c7c4ea7d5a911ae3d662e4", false },
    };
}
