using Repo = Kanban.Model.Dto.Repository;
using App = Kanban.Model.Dto.Application.Card;
using MongoDB.Bson.Serialization.Conventions;

namespace Kanban.Application.Tests.Services;

public class CardServiceTest
{
    private readonly Fixture fixture;
    private readonly Mock<ICardsDatabaseWorker> worker;
    private readonly Mock<IBoardsDatabaseWorker> boardWorker;
    private readonly CardService cardService;

    public CardServiceTest() 
    {
        this.fixture = new Fixture();
        this.worker = new Mock<ICardsDatabaseWorker>();
        this.boardWorker = new Mock<IBoardsDatabaseWorker>();
        this.cardService = new CardService(this.worker.Object, this.boardWorker.Object);
    }

    [Fact]
    public async void GetCardById_ShouldReturnCard_WhenThereIsACardInDatabase()
    {
        // Arrange
        var card = this.fixture.Create<Repo.Card.CardDto>();
        this.worker.Setup(x => x.GetCardById(card._id))
            .ReturnsAsync(card)
            .Verifiable();

        // Act
        var result = await this.cardService.GetCardById(card._id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(card._id);
        result.Name.Should().Be(card.Name); 
        result.Description.Should().Be(card.Description);
    }

    [Fact]
    public async void GetCardById_ShouldNotReturnCard_WhenThereIsNoCardInDatabase()
    {
        // Arrange
        var card = this.fixture.Create<Repo.Card.CardDto>();
        this.worker.Setup(x => x.GetCardById(card._id))
            .ReturnsAsync(card)
            .Verifiable();
        var nonexistentCardId = "non-existent-card-id";

        // Act
        var result = await this.cardService.GetCardById(nonexistentCardId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async void GetAllCards_ShouldReturnCards_WhenThereAreCardsInDatabase()
    {
        // Arrange
        var boardId = this.fixture.Create<string>();
        var board = this.fixture.Build<Repo.Board.BoardDto>()
            .With(x => x._id, boardId)
            .Create();
        var columnId = board.Columns.First()._id;
        var cards = this.fixture.Create<List<Repo.Card.CardDto>>();
        
        this.boardWorker.Setup(x => x.GetBoardById(board._id))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.GetAllCards(board.Columns.First().Cards))
            .ReturnsAsync(cards)
            .Verifiable();

        // Act
        var result = await this.cardService.GetCards(boardId,columnId);

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().BeNull();
        result.CardList.Count.Should().Be(3);
        result.CardList.Select(r => r.Id).Should().Equal(cards.Select(c => c._id));
        result.CardList.Select(r => r.Name).Should().Equal(cards.Select(c => c.Name));
        result.CardList.Select(r => r.Description).Should().Equal(cards.Select(c => c.Description));
    }

    [Fact]
    public async void GetAllCards_ShouldNotReturnCards_WhenBoardIsNotFound()
    {
        // Arrange
        var boardId = this.fixture.Create<string>();
        var columnId = this.fixture.Create<string>();

        this.boardWorker.Setup(x => x.GetBoardById(boardId))
            .Verifiable();

        // Act
        var result = await this.cardService.GetCards(boardId, columnId);

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be("Board not found");
    }

    [Fact]
    public async void GetAllCards_ShouldNotReturnCards_WhenBoardIsEmpty()
    {
        // Arrange
        var boardId = this.fixture.Create<string>();
        var board = this.fixture.Build<Repo.Board.BoardDto>()
            .With(x => x._id, boardId)
            .Create();
        var columnId = board.Columns.First()._id;

        this.boardWorker.Setup(x => x.GetBoardById(board._id))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.GetAllCards(board.Columns.First().Cards))
            .Verifiable();

        // Act
        var result = await this.cardService.GetCards(boardId, columnId);

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be("Cards not found");
    }

    [Fact]
    public async void CreateCard_ShouldCreateCard_WhenAValidCardIsGiven()
    {
        // Arrange
        var card = this.fixture.Create<Repo.Card.CardDto>();

        this.worker.Setup(x => x.InsertCard(It.Is<Repo.Card.CardDto>
            (x => x.Name == card.Name && x.Description == card.Description)))
            .ReturnsAsync(card)
            .Verifiable();

        var appCard = new App.CardDto
        {
            Name = card.Name,
            Description = card.Description,
        };

        // Act
        var result = await this.cardService.CreateCard(appCard);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(card._id);
        result.Name.Should().Be(card.Name);
        result.Description.Should().Be(card.Description);
    }

    [Fact]
    public async void DeleteCard_ShouldDeleteCard_WhenAValidCardIdIsGiven()
    {
        // Arrange
        var board = this.fixture.Create<Repo.Board.BoardDto>();
        var id = this.fixture.Create<string>();

        this.boardWorker.Setup(x => x.GetBoardById(board._id))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.DeleteById(id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var result = await this.cardService.DeleteCard(board._id, board.Columns.First()._id, id);

        // Assert
        result.Error.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(GetDeleteParameter))]
    public async void DeleteCard_ShouldNotDeleteCard_WhenInvalidRequestIsGiven(Repo.Board.BoardDto board, string cardId, string error, string boardId, string columnId)
    {
        // Arrange
        this.boardWorker.Setup(x => x.GetBoardById(board._id))
            .ReturnsAsync(board)
            .Verifiable();

        this.worker.Setup(x => x.DeleteById(cardId))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var result = await this.cardService.DeleteCard(boardId, columnId, cardId);

        // Assert
        result.Error.Should().Be(error);
    }

    private static IEnumerable<object[]> GetDeleteParameter()
    {
        var fix = new Fixture();
        var board = fix.Create<Repo.Board.BoardDto>();
        var id = fix.Create<string>();
        var boardWithNoColumn = fix.Build<Repo.Board.BoardDto>()
            .With(x => x.Columns, new Repo.Column.ColumnDto[0])
            .Create();

        var invalidId = fix.Create<string>();

        return new List<object[]>
        {
            new object[] { board, id, "Board not found", id, id },
            new object[] { boardWithNoColumn, boardWithNoColumn._id, "Column not found", boardWithNoColumn._id, id },
            new object[] { board, id, "Unable to delete card", board._id, board.Columns.First()._id  },
        };
    }

    [Fact]
    public async void UpdateCard_ShouldUpdateCard_WhenAValidCardIsGiven()
    {
        // Arrange
        var card = this.fixture.Create<Repo.Card.CardDto>();

        this.worker.Setup(x => x.UpdateCard(It.Is<Repo.Card.CardDto>
            (x => x._id == card._id && x.Name == card.Name && x.Description == card.Description)))
            .ReturnsAsync(card)
            .Verifiable();

        var appCard = new App.CardDto
        {
            Id = card._id,
            Name = card.Name,
            Description = card.Description,
        };

        // Act
        var result = await this.cardService.UpdateCard(appCard);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(card._id);
        result.Name.Should().Be(card.Name);
        result.Description.Should().Be(card.Description);
    }

    [Fact]
    public async void UpdateCard_ShouldNotUpdateCard_WhenAnInvalidCardIsGiven()
    {
        // Arrange
        var card = this.fixture.Create<Repo.Card.CardDto>();

        this.worker.Setup(x => x.UpdateCard(It.Is<Repo.Card.CardDto>
            (x => x._id == card._id && x.Name == card.Name && x.Description == card.Description)))
            .ReturnsAsync(card)
            .Verifiable();

        var appCard = new App.CardDto
        {
            Id = this.fixture.Create<string>(),
            Name = card.Name,
            Description = card.Description,
        };

        // Act
        var result = await this.cardService.UpdateCard(appCard);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(GetMoveCardParams))]
    public async void MoveCard_ShouldMoveCard_WhenValidRequestIsGiven(Repo.Board.BoardDto board, string boardId, Repo.Column.ColumnDto column, string card, bool? response, int index, string error)
    {
        // Arrange
        this.boardWorker.Setup(x => x.GetBoardById(boardId))
            .ReturnsAsync(board)
            .Verifiable();

        this.boardWorker.Setup(x => x.UpdateColumnCards(boardId, column))
            .ReturnsAsync(response)
            .Verifiable();

        // Act
        var result = await this.cardService.MoveCard(board._id, column._id, card, index);

        // Assert
        if (response is true)
        {
            result.Error.Should().BeNull();
        }
        else
        {
            result.Error.Should().NotBeNull();
            result.Error.Should().Be(error);
        }
    }

    private static IEnumerable<object[]> GetMoveCardParams()
    {
        var fix = new Fixture();
        var id = fix.Create<string>();
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
            new object[] { board, board._id, board.Columns.First(), board.Columns.First().Cards.First(), true, 0, "" },
            new object[] { board, id, board.Columns.First(), board.Columns.First().Cards.First(), false, 0, "Board not found" },
            new object[] { board, board._id, columnWithoutCard, board.Columns.First().Cards.First(), false, 0, "Column not found" },
            new object[] { board, board._id, board.Columns.First(), board.Columns.First().Cards.First(), false, 5, "Index out of boundary" },
            new object[] { board, board._id, board.Columns.First(), id, false, 0, "Card not found" },
            new object[] { board, board._id, board.Columns.First(), board.Columns.First().Cards.First(), null, 0, "Invalid BoardId" },
            new object[] { board, board._id, board.Columns.First(), board.Columns.First().Cards.First(), false, 0, "Failed to update column" },
        };
    }
}
