using FluentAssertions;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Integration.Tests.Helper;
using Kanban.Model.Dto.API.Board;
using Newtonsoft.Json;
using System.Text;

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

    [Fact]
    public async Task GetAllBoards_EndpointReturnsCorrectContent()
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.GetAsync("boards");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = JsonConvert.DeserializeObject<List<BoardDto>>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
    }


    [Fact]
    public async Task CreateCards_EndpointsReturnCorrectContent()
    {
        // Arrange
        var payload = JsonConvert.DeserializeObject<CreateBoardRequest>(Mocks.CreateBoardRequest);
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.PostAsync("boards", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = JsonConvert.DeserializeObject<CreateBoardResponse>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
        content.Board.Should().NotBeNull();
        content.Board.Id.Should().NotBeNull();
        content.Board.Name.Should().NotBeNull();
        content.Board.Name.Should().Be("Create Board");
        content.Board.Columns.Should().NotBeNull();
        content.Board.Columns.Count.Should().Be(0);
    }
    [Fact]
    public async Task CreateCard_EndpointsReturnError()
    {
        // Arrange
        var payload = new CreateBoardRequest { Name = "AB" };
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.PostAsync("boards", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        var content = JsonConvert.DeserializeObject<CreateBoardResponse>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
        content.Board.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCard_EndpointsReturnsCorrectContent()
    {
        // Arrange
        var payload = JsonConvert.DeserializeObject<UpdateBoardRequest>(Mocks.UpdateBoardRequest);
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.PutAsync($"boards/{Mocks.BoardOneId}", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = JsonConvert.DeserializeObject<UpdateBoardResponse>(response.Content.ReadAsStringAsync().Result);
        content.Should().NotBeNull();
        content.Board.Should().NotBeNull();
        content.Board.Name.Should().Be(payload.Name);
        content.Board.Id.Should().Be(payload.Id);
        content.Board.Columns.Count.Should().Be(0);
    }

    [Fact]
    public async Task UpdateCard_EndpointsReturnError()
    {
        // Arrange
        var payload = JsonConvert.DeserializeObject<UpdateBoardRequest>(Mocks.NonexistingUpdateBoardRequest);
        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.PutAsync($"boards/{Mocks.NonexistingUpdateBoardId}", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Theory]
    [MemberData(nameof(GetDeleteParameters))]
    public async Task DeleteCard_Endpoint(string url, bool success)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.DeleteAsync(url);

        // Assert
        response.IsSuccessStatusCode.Should().Be(success);
        if (!success)
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    private static IEnumerable<object[]> GetGetParameters() => new List<object[]>
    {
        new object[] { "Boards/65cbec3865a3b4fbed6945aa", true },
        new object[] { "Boards/65cbec3865a3b4fbed694", false },
        new object[] { "Boards/65c806377d5a911ae3d662f0", false },
    };

    private static IEnumerable<object[]> GetDeleteParameters() => new List<object[]>
    {
        new object[] { $"Boards/{Mocks.BoardTwoId}", false },
        new object[] { $"Boards/{Mocks.EmptyBoardId}", true },
    };
}
