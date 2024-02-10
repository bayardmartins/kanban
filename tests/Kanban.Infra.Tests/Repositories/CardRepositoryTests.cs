﻿namespace Kanban.Infra.Tests.Repositories;

public class CardRepositoryTests : MongoRepositoryTestsSetup
{
    [Fact]
    [MongoRepositoryTestsSetup]
    public async Task GetById_ShouldReturnCard_WhenValidCardIsSearched()
    {
        // Act
        var response = await this.cardWorker.GetCardById(Mocks.SampleMockOneId);

        // Assert
        response._id.Should().Be(Mocks.SampleMockOneId);
    }


    [Fact]
    [MongoRepositoryTestsSetup]
    public async Task GetById_ShouldNotReturnCard_WhenInvalidCardIsSearched()
    {
        // Act
        var response = await this.cardWorker.GetCardById(ObjectId.GenerateNewId().ToString());

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    [MongoRepositoryTestsSetup]
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
    [MongoRepositoryTestsSetup]
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
    [MongoRepositoryTestsSetup]
    public async Task Insert_ShouldInsertCard_WhenCardIsGiven()
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.InsertMockObject);

        // Act
        var response = await this.cardWorker.InsertCard(card);

        // Assert
        response.Should().NotBeNull();
        response._id.Should().Be(card._id);
    }

    [Fact]
    [MongoRepositoryTestsSetup]
    public async Task Update_ShouldUpdateCard_WhenValidCardIsGiven()
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockOne);
        card.Name = "New Name";

        // Act
        var response = await this.cardWorker.UpdateCard(card);
        var updatedCard = await this.cardWorker.GetCardById(card._id);
        // Assert
        response.Should().NotBeNull();
        response._id.Should().Be(card._id);
        updatedCard.Name.Should().Be(card.Name);
    }

    [Fact]
    [MongoRepositoryTestsSetup]
    public async Task Update_ShouldNotUpdateCard_WhenNonExistingCardIsGiven()
    {
        // Arrange
        var card = JsonConvert.DeserializeObject<CardDto>(Mocks.NonexistingMockObject);

        // Act
        var response = await this.cardWorker.UpdateCard(card);

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    [MongoRepositoryTestsSetup]
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
    [MongoRepositoryTestsSetup]
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
    }

    [Fact]
    [MongoRepositoryTestsSetup]
    public async Task Delete_ShouldNotDeleteCard_WhenUnexistingCardIdIsGiven()
    {
        // Arrange
        var id = Mocks.NonExistingCardId;

        // Act
        var response = await this.cardWorker.DeleteById(id);
        var deletedCard = await this.cardWorker.GetCardById(id);
        var remainingCards = await this.cardWorker.GetAllCards();
        // Assert
        response.Should().Be(false);
        deletedCard.Should().BeNull();
        remainingCards.Count.Should().Be(3);
    }

}