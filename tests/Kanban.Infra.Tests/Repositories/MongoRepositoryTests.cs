using MongoDB.Bson;

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
    }
}
