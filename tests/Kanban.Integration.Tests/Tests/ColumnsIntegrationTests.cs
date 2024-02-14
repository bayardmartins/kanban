using FluentAssertions;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Integration.Tests.Helper;
using Kanban.Model.Dto.API.Board;
using Kanban.Model.Dto.API.Column;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Kanban.Integration.Tests.Tests;

public class ColumnsIntegrationTests : IntegrationTestsSetup
{
    public ColumnsIntegrationTests(ApiWebApplicationFactory fixture) : base(fixture) { }

    [Theory]
    [MemberData(nameof(GetUpdateParameters))]
    public async Task AddColumn_EndpointsReturnCorrectContent(string mock, int index, int count)
    {
        // Arrange
        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var payload = JsonConvert.DeserializeObject<AddColumnRequest>(mock);

        using StringContent jsonContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync($"boards/{Mocks.BoardOneId}/columns", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        AuthenticationHelper.SetupAuthenticationHeader(_client, this.GetCredentials());
        var board = await _client.GetAsync($"boards/{Mocks.BoardOneId}");
        var content = JsonConvert.DeserializeObject<GetBoardResponse>(board.Content.ReadAsStringAsync().Result);
        content.Board.Columns.Count.Should().Be(count);
        content.Board.Columns.FindIndex(x => x.Name == payload.ColumnName).Should().Be(index);
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
    }

    private static IEnumerable<object[]> GetUpdateParameters() => new List<object[]>
    {
        new object[] { Mocks.AddColumnRequestOne, 0, 3 },
        new object[] { Mocks.AddColumnRequestTwo, 1, 4 },
        new object[] { Mocks.AddColumnRequestThree, 3, 5 },
    };
}
