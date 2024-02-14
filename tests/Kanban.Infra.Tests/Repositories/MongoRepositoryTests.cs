﻿using Kanban.Model.Dto.Repository.Client;

namespace Kanban.Infra.Tests.Repositories;

public class ClientRepositoryTests : MongoRepositoryTestsSetup
{
    #region Cards
    [Fact]
    public async Task GetById_ShouldReturnCard_WhenValidCardIsSearched()
    {
        // Act
        var response = await this.cardWorker.GetCardById(Mocks.SampleMockOneId);

        // Assert
        response._id.Should().Be(Mocks.SampleMockOneId);
    }

    [Fact]
    public async Task GetById_ShouldNotReturnCard_WhenInvalidCardIsSearched()
    {
        // Act
        var response = await this.cardWorker.GetCardById(ObjectId.GenerateNewId().ToString());

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ShouldReturnCards_WhenThereAreCardsInDatabase()
    {
        // Act
        var response = await this.cardWorker.GetAllCards();

        // Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(3);
        response.First(x => x._id == Mocks.SampleMockOneId).Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_ShouldReturnNoCards_WhenThereAreNoCardsInDatabase()
    {
        // Assert
        this.Dispose();

        // Act
        var response = await this.cardWorker.GetAllCards();

        // Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(0);
    }

    [Fact]
    public async Task Insert_ShouldInsertCard_WhenCardIsGiven()
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.InsertMockObject);

        // Act
        var response = await this.cardWorker.InsertCard(card);

        // Assert
        response.Should().NotBeNull();
        response._id.Should().NotBe(card._id);
    }

    [Fact]
    public async Task Update_ShouldUpdateCard_WhenValidCardIsGiven()
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.UpdateMockObject);
        card = await this.cardWorker.InsertCard(card);
        card.Name = "New Name";

        // Act
        var response = await this.cardWorker.UpdateCard(card);
        var updatedCard = await this.cardWorker.GetCardById(card._id);
        // Assert
        response.Should().NotBeNull();
        response._id.Should().Be(card._id);
        updatedCard.Name.Should().Be("New Name");
    }

    [Theory]
    [InlineData(Mocks.NonexistingMockObject)]
    [InlineData(Mocks.InvalidMockObject)]
    public async Task Update_ShouldNotUpdateCard_WhenNonExistingCardIsGiven(string mock)
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(mock);

        // Act
        var response = await this.cardWorker.UpdateCard(card);

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public async Task UpdateMany_ShouldUpdateManyCardDescriptions_WhenValidCardIdsAreGiven()
    {
        // Arrange
        var ids = new List<string> { Mocks.SampleMockOneId, Mocks.SampleMockTwoId };
        var newDescription = "New Description";

        // Act
        var response = await this.cardWorker.UpdateManyDescriptions(ids, newDescription);

        // Assert
        response.Should().Be(2);
    }

    [Fact]
    public async Task UpdateMany_ShouldNotUpdateAnyCardDescriptions_WhenAnInvalidValidCardIdsIsGiven()
    {
        // Arrange
        var ids = new List<string> { Mocks.SampleMockOneId, Mocks.SampleMockTwoId, Mocks.NonexistingMockObject };
        var newDescription = "New Description";

        // Act
        var response = await this.cardWorker.UpdateManyDescriptions(ids, newDescription);

        // Assert
        response.Should().Be(0);
    }

    [Fact]
    public async Task Delete_ShouldDeleteCard_WhenValidCardIdIsGiven()
    {
        // Arrange
        var id = Mocks.SampleMockOneId;

        // Act
        var response = await this.cardWorker.DeleteById(id);
        var deletedCard = await this.cardWorker.GetCardById(id);
        var remainingCards = await this.cardWorker.GetAllCards();
        // Assert
        response.Should().Be(true);
        deletedCard.Should().BeNull();
        remainingCards.Count.Should().Be(2);
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockOne);
        await this.cardWorker.InsertCard(card);
    }

    [Theory]
    [InlineData(Mocks.NonExistingCardId)]
    [InlineData(Mocks.InvalidId)]
    public async Task Delete_ShouldNotDeleteCard_WhenInvalidCardIdIsGiven(string id)
    {
        // Act
        var response = await this.cardWorker.DeleteById(id);
        var deletedCard = await this.cardWorker.GetCardById(id);
        var remainingCards = await this.cardWorker.GetAllCards();
        // Assert
        response.Should().Be(false);
        deletedCard.Should().BeNull();
        remainingCards.Count.Should().Be(3);
    }

    #endregion

    #region Auth
    [Fact]
    public async Task GetById_ShouldGetClient_WhenValidIdAreSend()
    {
        // Act
        var result = await this.authWorker.GetClientById("client");

        // Assert
        result.Should().NotBeNull();
        result.Secret.Should().Be("secret");
    }

    [Fact]
    public async Task GetById_ShouldNotGetClient_WhenInvalidIdAreSend()
    {
        // Act
        var result = await this.authWorker.GetClientById("fake_client");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Register_ShouldRegisterClient_WhenValidCredentialsAreSend()
    {
        // Arrange
        var client = JsonConvert.DeserializeObject<ClientDto>(Mocks.NewClientMock);

        // Act
        await this.authWorker.RegisterClient(client);
        var result = await this.authWorker.GetClientById(client._id);

        // Assert
        result.Should().NotBeNull();
        result.Secret.Should().Be("newsecret");
    }
    #endregion

    #region Boards
    [Fact]
    public async Task GetById_ShouldReturnBoard_WhenValidBoardIsSearched()
    {
        // Act
        var response = await this.boardWorker.GetBoardById(Mocks.BoardId);

        // Assert
        response._id.Should().Be(Mocks.BoardId);
    }

    [Theory]
    [InlineData("65c6e255a03db52a8056230f")]
    [InlineData("65c6e255a03db52a80562")]
    public async Task GetById_ShouldNotReturnBoard_WhenInvalidBoardIsSearched(string id)
    {
        // Act
        var response = await this.boardWorker.GetBoardById(id);

        // Assert
        response.Should().BeNull();
    }
    #endregion
}
