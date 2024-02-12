using Kanban.CrossCutting;
using Kanban.Integration.Tests.DatabaseMocks;
using Kanban.Repository.Dto.Models;
using Kanban.Repository.Repositories;
using Kanban.Repository.Settings;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Kanban.Integration.Tests;

public class IntegrationTestsSetup : IClassFixture<ApiWebApplicationFactory>
{
    protected readonly ApiWebApplicationFactory _factory;
    protected readonly HttpClient _client;
    private readonly MongoSettings _setting;
    private readonly IEnumerable<IMongoClient> _clients;

    public IntegrationTestsSetup(ApiWebApplicationFactory fixture)
    {
        _factory = fixture;
        _client = _factory.CreateClient();

        _setting = _factory.Configuration.GetSection(Constants.MongoSettings).Get<MongoSettings>();
        var kanbanSettings = MongoClientSettings.FromConnectionString(_setting.KanbanHost.Host); ;
        kanbanSettings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
        var client = new MongoClient(kanbanSettings);
        _clients = new List<IMongoClient> { client };
        this.Dispose();
        this.Migrate();
    }

    private void Migrate()
    {
        var cardCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<CardDto>(_setting.Collections.Cards);
        var cardOne = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockOne);
        var cardTwo = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockTwo);
        var cardThree = JsonConvert.DeserializeObject<CardDto>(Mocks.SampleMockThree);
        var list = new List<CardDto> { cardOne, cardTwo, cardThree };
        cardCollection.InsertMany(list);
        var clientCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<ClientDto>(_setting.Collections.Clients);
        var client = JsonConvert.DeserializeObject<ClientDto>(Mocks.ClientMock);
        clientCollection.InsertOne(client);
    }

    private void Dispose()
    {
        var collection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<CardDto>(_setting.Collections.Cards);
        collection.DeleteMany(Builders<CardDto>.Filter.Empty);
        var clientCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<ClientDto>(_setting.Collections.Clients);
        clientCollection.DeleteMany(Builders<ClientDto>.Filter.Empty);
    }
}
