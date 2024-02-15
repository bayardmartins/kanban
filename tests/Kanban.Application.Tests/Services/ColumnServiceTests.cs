using Repo = Kanban.Model.Dto.Repository;
using App = Kanban.Model.Dto.Application;
using Kanban.Model.Dto.Application.Column;

namespace Kanban.Application.Tests.Services;

public class ColumnServiceTests
{
    private readonly Fixture fixture;
    private readonly Mock<IBoardsDatabaseWorker> worker;
    private readonly ColumnService columnService;

    public ColumnServiceTests() 
    {
        this.fixture = new Fixture();
        this.worker = new Mock<IBoardsDatabaseWorker>();
        this.columnService = new ColumnService(this.worker.Object);
    }

    [Fact]
    public async void AddColumn_ShouldAddColumn_WhenValidColumnIsGiven()
    {
        // Arrange
        var request = this.fixture.Build<AddColumnRequest>()
            .With(x => x.Index, 1)
            .Create();
        var board = this.fixture.Build<Repo.Board.BoardDto>()
            .With(x => x._id, request.BoardId)
            .Create();

        this.worker.Setup(x => x.GetBoardById(request.BoardId))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.UpdateBoardColumns(It.Is<Repo.Board.BoardDto>
            (x => x._id == request.BoardId), 1))
            .ReturnsAsync(Guid.NewGuid().ToString())
            .Verifiable();

        // Act
        var result = await this.columnService.AddColumn(request);

        // Assert
        result.Error.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(GetAddColumnRequest))]
    public async void AddColumn_ShouldNotAddColumn_WhenInvalidColumnIsGiven(int index, string error, string boardId, string updateResponse)
    {
        // Arrange
        var request = this.fixture.Build<AddColumnRequest>()
            .With(x => x.Index, index)
            .With(x => x.BoardId, boardId)
            .Create();
        var board = this.fixture.Build<Repo.Board.BoardDto>()
            .With(x => x._id, request.BoardId)
            .Create();

        this.worker.Setup(x => x.GetBoardById(It.Is<string>(x => x == "boardIdOk" || x == "boardIdInvalid")))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.UpdateBoardColumns(It.Is<Repo.Board.BoardDto>
            (x => x._id == "boardIdOk"), index))
            .ReturnsAsync(updateResponse)
            .Verifiable();

        // Act
        var result = await this.columnService.AddColumn(request);

        // Assert
        result.Error.Should().Be(error);
    }

    [Theory]
    [MemberData(nameof(UpdateColumnData))]
    public async void UpdateColumn_ShouldUpdate_WhenValidRequestIsGiven(Repo.Board.BoardDto board, UpdateColumnRequest request, string response, bool? result)
    {
        // Arrange
        this.worker.Setup(x => x.GetBoardById(board._id))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.UpdateBoardColumnName(It.Is<Repo.Column.UpdateColumnRequest>
            (x => x.ColumnId == request.ColumnId && x.BoardId == request.BoardId)))
            .ReturnsAsync(result)
            .Verifiable();

        // Act
        var updateResult = await this.columnService.UpdateColumn(request);

        // Assert
        if (result is not null && result == true)
        {
            updateResult.Error.Should().BeNull();
        }
        else
        {
            updateResult.Error.Should().Be(response);
        }
    }

    private static IEnumerable<object[]> GetAddColumnRequest() => new List<object[]>
    {
        new object[] { 4, "Index out of boundary", "boardIdOk", "d329cada-fe7f-4378-82df-56dea642627a" },
        new object[] { 3, "Board not found", "boardIdNotFound", "d329cada-fe7f-4378-82df-56dea642627a" },
        new object[] { 3, "Invalid board id", "boardIdInvalid", "d329cada-fe7f-4378-82df-56dea642627a" },
        new object[] { 3, "Failed to update", "boardIdOk", "" },
    };

    private static IEnumerable<object[]> UpdateColumnData()
    {
        var fix = new Fixture();

        var board = fix.Create<Repo.Board.BoardDto>();
        var boardNotFoundRequest = fix.Build<UpdateColumnRequest>()
            .With(x => x.ColumnId, board.Columns.First()._id)
            .Create();
        var columnNotFoundRequest = fix.Build<UpdateColumnRequest>()
            .With(x => x.BoardId, board._id)
            .Create();
        var request = fix.Build<UpdateColumnRequest>()
            .With(x => x.BoardId, board._id)
            .With(x => x.ColumnId, board.Columns.First()._id)
            .Create();

        return new List<object[]>
        {
            new object[] { board, boardNotFoundRequest, "Board not found", false },
            new object[] { board, columnNotFoundRequest, "Column not found", false },
            new object[] { board, request, "Column not found", false },
            new object[] { board, request, "Invalid Id", null },
            new object[] { board, request, null, true },
        };
    }
}
