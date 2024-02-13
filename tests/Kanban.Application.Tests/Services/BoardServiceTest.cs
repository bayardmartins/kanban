using Repo = Kanban.Model.Dto.Repository.Board;
using App = Kanban.Model.Dto.Application.Board;

namespace Kanban.Application.Tests.Services;

public class BoardServiceTest
{
    private readonly Fixture fixture;
    private readonly Mock<IBoardsDatabaseWorker> worker;
    private readonly BoardService boardService;

    public BoardServiceTest()
    {
        this.fixture = new Fixture();
        this.worker = new Mock<IBoardsDatabaseWorker>();
        this.boardService = new BoardService(this.worker.Object);
    }

    [Fact]
    public async void GetBoards_ShouldReturnBoard_WhenThereIsBoardInDatabase()
    {
        // Arrange
        var board = this.fixture.Create<Repo.BoardDto>();
        this.worker.Setup(x => x.GetBoardById(board._id))
            .ReturnsAsync(board)
            .Verifiable();

        // Act
        var result = await this.boardService.GetBoard(board._id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(board._id);
        result.Name.Should().Be(board.Name);
        result.Columns.Length.Should().Be(board.Columns.Length);
        for(int i = 0; i > result.Columns.Length; i++)
        {
            result.Columns[i].Id.Should().Be(board.Columns[i]._id);
            result.Columns[i].Name.Should().Be(board.Columns[i].Name);
            result.Columns[i].Cards.Should().BeEquivalentTo(board.Columns[i].Cards);
        }
    }
}
