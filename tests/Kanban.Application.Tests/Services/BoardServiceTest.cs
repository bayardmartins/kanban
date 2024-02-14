using Repo = Kanban.Model.Dto.Repository.Board;
using App = Kanban.Model.Dto.Application.Board;
using Api = Kanban.Model.Dto.API.Board;

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
        result.Columns.Count.Should().Be(board.Columns.Length);
        for(int i = 0; i > result.Columns.Count; i++)
        {
            result.Columns[i].Id.Should().Be(board.Columns[i]._id);
            result.Columns[i].Name.Should().Be(board.Columns[i].Name);
            result.Columns[i].Cards.Should().BeEquivalentTo(board.Columns[i].Cards);
        }
    }

    [Fact]
    public async void CreateBoard_ShouldCreateBoard_WhenValidRequestIsMade()
    {
        // Arrange
        var board = this.fixture.Build<Repo.BoardDto>()
            .With(x => x.Columns, new Repo.ColumnDto[0])
            .Create();

        var appBoard = new App.BoardDto
        {
            Name = board.Name,
        };

        this.worker.Setup(x => x.InsertBoard(It.Is<Repo.BoardDto>(x => x.Name == board.Name)))
            .ReturnsAsync(board)
            .Verifiable();
        // Act
        var result = await this.boardService.CreateBoard(appBoard);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(board._id);
        result.Name.Should().Be(board.Name);
        result.Columns.Should().NotBeNull();
        result.Columns.Count.Should().Be(0);
    }

    [Fact]
    public async void UpdateBoard_ShouldUpdateBoard_WhenValidRequestIdMade()
    {
        // Arrange
        var board = this.fixture.Build<Repo.BoardDto>()
            .With(x => x.Columns, new Repo.ColumnDto[0])
            .Create();

        var appBoard = new App.BoardDto
        {
            Id = board._id,
            Name = board.Name,
        };

        this.worker.Setup(x => x.UpdateBoard(It.Is<Repo.BoardDto>(x => x.Name == board.Name && x._id == board._id)))
            .ReturnsAsync(board)
            .Verifiable();

        // Act
        var result = await this.boardService.UpdateBoard(appBoard);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(board._id);
        result.Name.Should().Be(board.Name);
        result.Columns.Should().NotBeNull();
        result.Columns.Count.Should().Be(0);
    }

    [Fact]
    public async void UpdateBoard_ShouldNotUpdateBoard_WhenInalidRequestIdMade()
    {
        // Arrange
        var board = this.fixture.Build<Repo.BoardDto>()
            .With(x => x.Columns, new Repo.ColumnDto[0])
            .Create();

        var appBoard = new App.BoardDto
        {
            Id = this.fixture.Create<string>(),
            Name = board.Name,
        };

        this.worker.Setup(x => x.UpdateBoard(It.Is<Repo.BoardDto>(x => x.Name == board.Name && x._id == board._id)))
            .ReturnsAsync(board)
            .Verifiable();

        // Act
        var result = await this.boardService.UpdateBoard(appBoard);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async void DeleteBoard_ShouldDeleteCard_WhenValidBoardIsGiven()
    {
        // Arrange
        var id = this.fixture.Create<string>();
        this.worker.Setup(x => x.DeleteById(id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var result = await this.boardService.DeleteBoard(id);

        // Assert
        result.Should().BeTrue();
    }    
}
