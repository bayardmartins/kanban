using FluentAssertions;
using Kanban.Integration.Tests.Helper;
using Kanban.Model.Dto.API.Board;
using Kanban.Model.Dto.API.Card;
using Newtonsoft.Json;

namespace Kanban.Integration.Tests.Tests;

public class BoardsIntegrationTests : IntegrationTestsSetup
{
    public BoardsIntegrationTests(ApiWebApplicationFactory fixture) : base(fixture) { }

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
        var content = JsonConvert.DeserializeObject<GetBoardResponse>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
        if (success) 
        { 
            content.Board.Should().NotBeNull(); 
        }
        else 
        { 
            content.Board.Should().BeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound); 
        }
    }

    private static IEnumerable<object[]> GetGetParameters() => new List<object[]>
    {
        new object[] { "Boards/65cbec3865a3b4fbed6945aa", true },
        new object[] { "Boards/65cbec3865a3b4fbed694", false },
        new object[] { "Boards/65c806377d5a911ae3d662f0", false },
    };
}
