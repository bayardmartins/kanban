using AutoFixture;
using App = Kanban.Application.Dto.Models;
using Repo = Kanban.Repository.Dto.Models;

namespace Kanban.Application.Tests.Services;

public class AuthServiceTest
{
    private readonly Fixture fixture;
    private readonly Mock<IAuthDatabaseWorker> worker;
    private readonly AuthService cardService;

    public AuthServiceTest()
    {
        this.fixture = new Fixture();
        this.worker = new Mock<IAuthDatabaseWorker>();
        this.cardService = new AuthService(this.worker.Object);
    }

    [Fact]
    public async void Register_ShouldRegister_WhenCalled()
    {
        // Arrange
        var repoClient = this.fixture.Create<Repo.ClientDto>();
        var appClient = new App.ClientDto
        {
            Id = repoClient._id,
            Secret = repoClient.Secret,
        };
        this.worker.Setup(x => x.RegisterClient(repoClient))
            .Verifiable();

        // Act
        Func<Task> act = async () => await this.cardService.RegisterClient(appClient);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async void Login_ShouldLoginSuccessfully_WhenValidClientAndSecretIsGiven()
    {
        // Arrange
        var repoClient = this.fixture.Create<Repo.ClientDto>();
        var appClient = new App.ClientDto
        {
            Id = repoClient._id,
            Secret = repoClient.Secret,
        };
        this.worker.Setup(x => x.GetClientById(appClient.Id))
            .ReturnsAsync(repoClient)
            .Verifiable();

        // Act
        var result = await this.cardService.Login(appClient);

        // Assert
        result.Should().Be(true);
    }

    [Fact]
    public async void Login_ShouldNotLoginSuccessfully_WhenInvalidClientAndSecretIsGiven()
    {
        // Arrange
        var repoClient = this.fixture.Create<Repo.ClientDto>();
        var appClient = new App.ClientDto
        {
            Id = repoClient._id,
            Secret = this.fixture.Create<string>(),
        };
        this.worker.Setup(x => x.GetClientById(appClient.Id))
            .ReturnsAsync(repoClient)
            .Verifiable();

        // Act
        var result = await this.cardService.Login(appClient);

        // Assert
        result.Should().Be(false);
    }

    [Fact]
    public async void Login_ShouldNotLoginSuccessfully_WhenNonExistingClientIsGiven()
    {
        // Arrange
        var repoClient = this.fixture.Create<Repo.ClientDto>();
        var appClient = new App.ClientDto
        {
            Id = repoClient._id,
            Secret = this.fixture.Create<string>(),
        };
        this.worker.Setup(x => x.GetClientById(repoClient._id))
            .Verifiable();

        // Act
        var result = await this.cardService.Login(appClient);

        // Assert
        result.Should().Be(false);
    }
}
