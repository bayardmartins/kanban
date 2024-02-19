using FluentAssertions;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Integration.Tests.Helper;
using Kanban.Model.Dto.API.Board;
using Kanban.Model.Dto.API.Column;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Kanban.Integration.Tests.Tests;

public class ColumnsIntegrationTests : IntegrationTestsSetup
{
    public ColumnsIntegrationTests(ApiWebApplicationFactory fixture) : base(fixture) { }

    [Fact]
    public async Task AddColumn_EndpointsReturnCorrectContent()
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = JsonConvert.DeserializeObject<AddColumnRequest>(Mocks.AddColumnRequestOne);

        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var boardPre = await _client.GetAsync($"boards/{Mocks.BoardOneId}");
        var contentPre = JsonConvert.DeserializeObject<GetBoardResponse>(boardPre.Content.ReadAsStringAsync().Result);
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        
        // Act
        var response = await _client.PostAsync($"boards/{Mocks.BoardOneId}/columns", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var boardPost = await _client.GetAsync($"boards/{Mocks.BoardOneId}");
        var contentPost = JsonConvert.DeserializeObject<GetBoardResponse>(boardPost.Content.ReadAsStringAsync().Result);
        contentPost.Board.Columns.Count.Should().Be(contentPre.Board.Columns.Count+1);
        contentPost.Board.Columns.FindIndex(x => x.Name == payload.ColumnName).Should().Be(0);
    }

    [Fact]
    public async Task AddColumn_EndpointReturnNotFound()
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = JsonConvert.DeserializeObject<AddColumnRequest>(Mocks.AddColumnRequestOne);

        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync($"boards/{Mocks.NonexistingBoardId}/columns", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(GetUpdateColumnParameters))]
    public async Task UpdateColumns_EndpointReturnSuccess(string boardId, bool result)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = JsonConvert.DeserializeObject<UpdateColumnRequest>(Mocks.UpdColumnReqSuccess);

        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"boards/{boardId}/columns/{Mocks.ExistingColumn}", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(GetDeleteColumnParameters))]
    public async Task DeleteColumn_EndpointReturnSuccess(string boardId, string columnId, bool result)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.DeleteAsync($"boards/{boardId}/columns/{columnId}");

        // Assert
        response.IsSuccessStatusCode.Should().Be(result);
    }

    [Theory]
    [MemberData(nameof(GetMoveColumnInBoardParameters))]
    public async Task MoveColumnInBoard_EndpointReturnSuccess(string url, bool result)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());

        // Act
        var response = await _client.PutAsync(url, null);

        // Assert
        if (result)
        {
            response.IsSuccessStatusCode.Should().BeTrue();
        }
        else
        {
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }

    private static IEnumerable<object[]> GetMoveColumnInBoardParameters() => new List<object[]>
    {
        new object[] { $"boards/{Mocks.BoardOneId}/columns/{Mocks.ExistingColumn}/index/{1}", true },
        new object[] { $"boards/{Mocks.BoardTwoId}/columns/{Mocks.ExistingColumn}/index/{0}", false },
    };
    private static IEnumerable<object[]> GetUpdateParameters() => new List<object[]>
    {
        new object[] { Mocks.AddColumnRequestOne, 0 },
        new object[] { Mocks.AddColumnRequestTwo, 1 },
        new object[] { Mocks.AddColumnRequestThree, 3 },
    };
    private static IEnumerable<object[]> GetUpdateColumnParameters() => new List<object[]>
    {
        new object[] { Mocks.BoardOneId, true },
        new object[] { Mocks.NonexistingBoardId, false },
    };
    private static IEnumerable<object[]> GetDeleteColumnParameters() => new List<object[]>
    {
        new object[] { Mocks.BoardOneId, Mocks.ExistingColumn, false },
        new object[] { Mocks.BoardOneId, Mocks.ColumnWithoutCards, true },
    };
}
