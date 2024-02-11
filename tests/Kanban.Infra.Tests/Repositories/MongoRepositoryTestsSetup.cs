using Kanban.Repository.Repositories;
using Kanban.Repository.Settings;
using Kanban.Repository.Worker;
using MongoDB.Driver;

namespace Kanban.Infra.Tests.Repositories;

public class MongoRepositoryTestsSetup : IDisposable
{
    public readonly KanbanDatabaseWorker cardWorker;
    public readonly AuthDatabaseWorker authWorker;
    private readonly MongoRepository _repository;
    private readonly MongoSettings _setting;
    private readonly IEnumerable<IMongoClient> _clients;

    public MongoRepositoryTestsSetup()
    {
        _setting = new MongoSettings
        {
            KanbanHost = new MongoHost
            {
                Host = "mongodb://localhost:27017",
                Database = "test",
                ClusterId = 0,
            },
            Collections = new Collections
            {
                Cards = "cards",
                Clients = "clients",
            },
        };
        var kanbanSettings = MongoClientSettings.FromConnectionString(_setting.KanbanHost.Host);;
        kanbanSettings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
        var client = new MongoClient(kanbanSettings);
        _clients = new List<IMongoClient> { client };
        _repository = new MongoRepository(_clients);

        this.cardWorker = new KanbanDatabaseWorker(_repository, _setting);
        this.authWorker = new AuthDatabaseWorker(_repository, _setting);
        this.Dispose();
        this.Migrate();
    }

    public void Migrate()
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

    public void Dispose()
    {
        var collection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<CardDto>(_setting.Collections.Cards);
        collection.DeleteMany(Builders<CardDto>.Filter.Empty);

        var clientCollection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<ClientDto>(_setting.Collections.Clients);
        clientCollection.DeleteMany(Builders<ClientDto>.Filter.Empty);
    }
}
