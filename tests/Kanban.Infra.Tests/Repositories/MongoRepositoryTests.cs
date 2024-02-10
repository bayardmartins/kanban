namespace Kanban.Infra.Tests.Repositories
{
    public class MongoRepositoryTests : MongoRepositoryTestsSetup
    {

        public MongoRepositoryTests() { }

        [Fact]
        [MongoRepositoryTestsSetup]
        public async Task GetById_ShouldReturnCard_WhenValidCardIsSearched()
        {
            // Act
            var response = await this.worker.GetCardById(CardMocks.SampleMockOneId);

            // Assert
            response._id.Should().Be(CardMocks.SampleMockOneId);
        }


        [Fact]
        [MongoRepositoryTestsSetup]
        public async Task GetById_ShouldNotReturnCard_WhenInvalidCardIsSearched()
        {
            // Act
            var response = await this.worker.GetCardById(ObjectId.GenerateNewId().ToString());

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        [MongoRepositoryTestsSetup]
        public async Task GetAll_ShouldReturnCards_WhenThereAreCardsInDatabase()
        {
            // Act
            var response = await this.worker.GetAllCards();

            // Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(3);
            response.First(x => x._id == CardMocks.SampleMockOneId).Should().NotBeNull();
        }

        [Fact]
        [MongoRepositoryTestsSetup]
        public async Task Insert_ShouldInsertCard_WhenCardIsGiven()
        {
            // Arrange
            var card = JsonConvert.DeserializeObject<CardDto>(CardMocks.InsertMockObject);

            // Act
            var response = await this.worker.InsertCard(card);

            // Assert
            response.Should().NotBeNull();
            response._id.Should().Be(card._id);
        }

        [Fact]
        [MongoRepositoryTestsSetup]
        public async Task Update_ShouldUpdateCard_WhenValidCardIsGiven()
        {
            // Arrange
            var card = JsonConvert.DeserializeObject<CardDto>(CardMocks.SampleMockOne);
            card.Name = "New Name";

            // Act
            var response = await this.worker.UpdateCard(card);
            var updatedCard = await this.worker.GetCardById(card._id);
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
            var card = JsonConvert.DeserializeObject<CardDto>(CardMocks.InsertMockObject);

            // Act
            var response = await this.worker.UpdateCard(card);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        [MongoRepositoryTestsSetup]
        public async Task UpdateMany_ShouldUpdateManyCardDescriptions_WhenValidCardIdsAreGiven()
        {
            // Arrange
            var ids = new List<string> { CardMocks.SampleMockOneId, CardMocks.SampleMockTwoId };
            var newDescription = "New Description";

            // Act
            var response = await this.worker.UpdateManyDescriptions(ids, newDescription);

            // Assert
            response.Should().Be(2);
        }
    }
}
