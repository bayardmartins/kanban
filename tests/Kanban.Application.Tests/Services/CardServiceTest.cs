using Repo = Kanban.Model.Dto.Repository.Card;
using App = Kanban.Model.Dto.Application.Card;

namespace Kanban.Application.Tests.Services;

public class CardServiceTest
{
    private readonly Fixture fixture;
    private readonly Mock<ICardsDatabaseWorker> worker;
    private readonly CardService cardService;

    public CardServiceTest() 
    {
        this.fixture = new Fixture();
        this.worker = new Mock<ICardsDatabaseWorker>();
        this.cardService = new CardService(this.worker.Object);
    }

    [Fact]
    public async void GetCardById_ShouldReturnCard_WhenThereIsACardInDatabase()
    {
        // Arrange
        var card = this.fixture.Create<Repo.CardDto>();
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
        var card = this.fixture.Create<Repo.CardDto>();
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
        var cards = this.fixture.Create<List<Repo.CardDto>>();
        this.worker.Setup(x => x.GetAllCards())
            .ReturnsAsync(cards)
            .Verifiable();

        // Act
        var result = await this.cardService.GetCards();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
        result.Select(r => r.Id).Should().Equal(cards.Select(c => c._id));
        result.Select(r => r.Name).Should().Equal(cards.Select(c => c.Name));
        result.Select(r => r.Description).Should().Equal(cards.Select(c => c.Description));
    }

    [Fact]
    public async void GetAllCards_ShouldNotReturnCards_WhenThereAreNoCardsInDatabase()
    {
        // Arrange
        this.worker.Setup(x => x.GetAllCards())
            .Verifiable();

        // Act
        var result = await this.cardService.GetCards();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
    }

    [Fact]
    public async void CreateCard_ShouldCreateCard_WhenAValidCardIsGiven()
    {
        // Arrange
        var card = this.fixture.Create<Repo.CardDto>();

        this.worker.Setup(x => x.InsertCard(It.Is<Repo.CardDto>
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
        var id = this.fixture.Create<string>();
        this.worker.Setup(x => x.DeleteById(id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var result = await this.cardService.DeleteCard(id);

        // Assert
        result.Should().Be(true);
    }

    [Fact]
    public async void UpdateCard_ShouldUpdateCard_WhenAValidCardIsGiven()
    {
        // Arrange
        var card = this.fixture.Create<Repo.CardDto>();

        this.worker.Setup(x => x.UpdateCard(It.Is<Repo.CardDto>
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
        var card = this.fixture.Create<Repo.CardDto>();

        this.worker.Setup(x => x.UpdateCard(It.Is<Repo.CardDto>
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
}
