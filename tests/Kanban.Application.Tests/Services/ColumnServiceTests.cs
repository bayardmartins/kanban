using Repo = Kanban.Model.Dto.Repository.Board;
using App = Kanban.Model.Dto.Application.Board;

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
        var request = this.fixture.Build<App.ColumnAddRequest>()
            .With(x => x.Index, 1)
            .Create();
        var board = this.fixture.Build<Repo.BoardDto>()
            .With(x => x._id, request.BoardId)
            .Create();

        this.worker.Setup(x => x.GetBoardById(request.BoardId))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.UpdateBoardColumns(It.Is<Repo.BoardDto>
            (x => x._id == request.BoardId), 1))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var result = await this.columnService.AddColumn(request);

        // Assert
        result.Error.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(GetAddColumnRequest))]
    public async void AddColumn_ShouldNotAddColumn_WhenValidColumnIsGiven(int index, string error, string boardId)
    {
        // Arrange
        var request = this.fixture.Build<App.ColumnAddRequest>()
            .With(x => x.Index, index)
            .With(x => x.BoardId, boardId)
            .Create();
        var board = this.fixture.Build<Repo.BoardDto>()
            .With(x => x._id, request.BoardId)
            .Create();

        this.worker.Setup(x => x.GetBoardById(It.Is<string>(x => x == "boardIdOk" || x == "boardIdInvalid")))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.UpdateBoardColumns(It.Is<Repo.BoardDto>
            (x => x._id == "boardId"), index))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var result = await this.columnService.AddColumn(request);

        // Assert
        result.Error.Should().Be(error);
    }


    private static IEnumerable<object[]> GetAddColumnRequest() => new List<object[]>
    {
        new object[] { 4, "Index out of boundary", "boardIdOk" },
        new object[] { 3, "Board not found", "boardIdNotFound" },
        new object[] { 3, "Invalid board id", "boardIdInvalid" },
    };
}
