using Repo = Kanban.Model.Dto.Repository;
using App = Kanban.Model.Dto.Application;
using Kanban.Model.Dto.Application.Column;
using Moq;
using System.Runtime.CompilerServices;

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

    [Fact]
    public async void DeleteColumn_ShouldDelete_WhenValidRequestIsGiven()
    {
        // Arrange
        var board = this.fixture.Create<Repo.Board.BoardDto>();
        board.Columns.First().Cards = new string[] { };

        var boardId = board._id;
        var columnId = board.Columns.First()._id;

        this.worker.Setup(x => x.GetBoardById(boardId))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.DeleteColumn(boardId, columnId))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var response = await this.columnService.DeleteColumn(boardId, columnId);

        // Assert
        response.Error.Should().BeNull();
        response.ColumnId.Should().Be(columnId);
    }

    [Theory]
    [MemberData(nameof(GetDeleteColumnParams))]
    public async void DeleteColumn_ShouldNotDelete_WhenInvalidRequestIsGiven(Repo.Board.BoardDto board, string boardId, string columnId, string error, bool? deleteResponse)
    {
        // Arrange
        this.worker.Setup(x => x.GetBoardById(board._id))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.DeleteColumn(boardId, columnId))
            .ReturnsAsync(deleteResponse)
            .Verifiable();

        // Act
        var response = await this.columnService.DeleteColumn(boardId, columnId);

        // Assert
        response.Error.Should().Be(error);
        response.ColumnId.Should().BeNull();
    }

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
    private static IEnumerable<object[]> GetAddColumnRequest() => new List<object[]>
    {
        new object[] { 4, "Index out of boundary", "boardIdOk", "d329cada-fe7f-4378-82df-56dea642627a" },
        new object[] { 3, "Board not found", "boardIdNotFound", "d329cada-fe7f-4378-82df-56dea642627a" },
        new object[] { 3, "Invalid board id", "boardIdInvalid", "d329cada-fe7f-4378-82df-56dea642627a" },
        new object[] { 3, "Failed to update", "boardIdOk", "" },
    };

    private static IEnumerable<object[]> GetDeleteColumnParams()
    {
        var fix = new Fixture();

        var board = fix.Create<Repo.Board.BoardDto>();
        var columnWithoutCard = fix.Build<Repo.Column.ColumnDto>()
            .With(x => x.Cards, new string[0])
            .Create();
        var columnsWithoutCard = new Repo.Column.ColumnDto[] { columnWithoutCard };
        var boardWithNoCards = fix.Build<Repo.Board.BoardDto>()
            .With(x => x.Columns, columnsWithoutCard)
            .Create();

        var invalidId = fix.Create<string>();

        return new List<object[]>
        {
            new object[] { board, invalidId, board.Columns.First()._id, "Board not found", false },
            new object[] { board, board._id, invalidId, "Column not found", false },
            new object[] { board, board._id, board.Columns.First()._id, "Column with cards can't be deleted. Column has 3 cards. Delete all cards before deleting column", false },
            new object[] { boardWithNoCards, boardWithNoCards._id, boardWithNoCards.Columns.First()._id, "BoardId invalid", null },
            new object[] { boardWithNoCards, boardWithNoCards._id, boardWithNoCards.Columns.First()._id, "Failed to delete column", false },
        };
    }
}
