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
    [MemberData(nameof(GetByIdParameters))]
    public async Task GetCardsById_EndpointsReturnCorrectContent(string url, bool success)
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
        else { content.Cards.Count.Should().Be(0); content.Error.Should().Be("Card not found"); }
    }

    [Theory]
    [MemberData(nameof(GetAllParameters))]
    public async Task GetAllCards_EndpointsReturnCorrectContent(string url, string error, bool success)
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
        else { content.Error.Should().Be(error); content.Cards.Count.Should().Be(0); }
    }

    [Fact]
    public async Task CreateCards_EndpointsReturnSuccessAndCorrectContent()
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = JsonConvert.DeserializeObject<CreateCardRequest>(Mocks.InsertMockObject);
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("Boards/65c6e255a03db52a8056230f/Columns/65cbec3865a3b4fbed6945aa/Cards", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = JsonConvert.DeserializeObject<CreateCardResponse>(response.Content.ReadAsStringAsync().Result);
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
    public async Task UpdateCards_EndpointsReturnSuccessOrFail(string url, string mock, bool result)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var card = JsonConvert.DeserializeObject<CardDto>(mock);
        card.Name = "new name";
        var payload = new UpdateCardRequest { Card = card };
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync(url, jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().Be(result);
    }
    private static IEnumerable<object[]> GetAllParameters() => new List<object[]>
    {
        new object[] { "Boards/65cbec3865a3b4fbed6945aa/Columns/65cbec6d65a3b4fbed6945ab/Cards", "", true },
        new object[] { "Boards/65cbec3865a3b4fbed6945aa/Columns/65cbec3865a3b4fbed6945aa/Cards", "Column not found", false },
        new object[] { "Boards/65ccad2765a3b4fbed6945b0/Columns/65cbec3865a3b4fbed6945aa/Cards", "Board not found", false },
    };

    private static IEnumerable<object[]> GetByIdParameters() => new List<object[]>
    {
        new object[] { "Cards/65c6e255a03db52a8056230f", true },
        new object[] { "Cards/65c77bbb7d5a911ae3d66", false },
        new object[] { "Cards/65cbec3865a3b4fbed6945aa", false },
    };
    private static IEnumerable<object[]> GetDeleteParameters() => new List<object[]>
    {
        new object[] { "Boards/65cbec3865a3b4fbed6945aa/Columns/65cbec6d65a3b4fbed6945ab/Cards/65c6e255a03db52a8056230f", true },
        new object[] { "Boards/65cbec3865a3b4fbed6945aa/Columns/65cbec6d65a3b4fbed6945ab/Cards/65c7c4ea7d5a911ae3d662e4", false },
    };

    private static IEnumerable<object[]> GetUpdateParameters() => new List<object[]>
    {
        new object[] { "Cards/65c77ba67d5a911ae3d662db", Mocks.UpdateMock, true },
        new object[] { "Cards/65c806377d5a911ae3d662f0", Mocks.NonexistingMockObject, false },
        new object[] { "Cards/65c806377d5a911ae3d662f0", Mocks.UpdateMock, false },
    };
}
