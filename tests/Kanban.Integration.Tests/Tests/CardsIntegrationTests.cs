using FluentAssertions;
using Kanban.Model.Dto.API.Card;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Integration.Tests.Helper;
using Newtonsoft.Json;
using System.Text;

namespace Kanban.Integration.Tests.Tests;

public class CardsIntegrationTests : IntegrationTestsSetup
{

    public CardsIntegrationTests(ApiWebApplicationFactory fixture) : base(fixture) { }

    [Theory]
    [MemberData(nameof(GetGetParameters))]
    public async Task GetCards_EndpointsReturnCorrectContent(string url, bool success)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.IsSuccessStatusCode.Should().Be(success);
        var content = JsonConvert.DeserializeObject<GetCardResponse>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
        if (success) { content.Cards.Count.Should().NotBe(0); }
        else { content.Cards.Count.Should().Be(0); }
    }

    [Fact]
    public async Task CreateCards_EndpointsReturnSuccessAndCorrectContent()
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = new CreateCardRequest { Card = JsonConvert.DeserializeObject<CardDto>(Mocks.InsertMockObject) };
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("Cards", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = JsonConvert.DeserializeObject<CreateCardResponse>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
        content.CreatedCard.Should().NotBeNull();
        content.CreatedCard.Id.Should().NotBeNullOrEmpty();
        content.CreatedCard.Name.Should().Be(payload.Card.Name);
        content.CreatedCard.Description.Should().Be(payload.Card.Description);
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
    public async Task UpdateCards_EndpointsReturnSuccessOrFail(string url, string mock, bool result)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = new UpdateCardRequest { Card = JsonConvert.DeserializeObject<CardDto>(mock) };
        payload.Card.Name = "new name";
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync(url, jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().Be(result);
    }
        private static IEnumerable<object[]> GetGetParameters() => new List<object[]>
    {
        new object[] { "Cards", true },
        new object[] { "Cards/65c77bbb7d5a911ae3d662dc", true },
        new object[] { "Cards/65c6e255a03db52a8056", false },
    };

    private static IEnumerable<object[]> GetDeleteParameters() => new List<object[]>
    {
        new object[] { "Cards/65c6e255a03db52a8056230f", true },
        new object[] { "Cards/65c7c4ea7d5a911ae3d662e4", false },
    };

    private static IEnumerable<object[]> GetUpdateParameters() => new List<object[]>
    {
        new object[] { "Cards/65c77ba67d5a911ae3d662db", Mocks.UpdateMock, true },
        new object[] { "Cards/65c7c4ea7d5a911ae3d662e4", Mocks.UpdateMock, false },
        new object[] { "Cards/65c806377d5a911ae3d662f0", Mocks.NonexistingMockObject, false },
    };
}
