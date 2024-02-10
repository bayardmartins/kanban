namespace Kanban.Infra.Tests.Repositories;

public class ClientRepositoryTests : MongoRepositoryTestsSetup
{
    [Fact]
    [MongoRepositoryTestsSetup]
    public async Task GetById_ShouldGetClient_WhenValidIdAreSend()
    {
        // Act
        var result = await this.authWorker.GetClientById("client");

        // Assert
        result.Should().NotBeNull();
        result.Secret.Should().Be("secret");
    }

    [Fact]
    [MongoRepositoryTestsSetup]
    public async Task GetById_ShouldNotGetClient_WhenInvalidIdAreSend()
    {
        // Act
        var result = await this.authWorker.GetClientById("fake_client");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    [MongoRepositoryTestsSetup]
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
}
