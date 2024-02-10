using Kanban.Repository.Repositories;
using Kanban.Repository.Settings;
using Kanban.Repository.Worker;
using MongoDB.Driver;
using System.Reflection;
using Xunit.Sdk;

namespace Kanban.Infra.Tests.Repositories;

public class MongoRepositoryTestsSetup : BeforeAfterTestAttribute
{
    public readonly KanbanDatabaseWorker worker;
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
            },
        };
        var kanbanSettings = MongoClientSettings.FromConnectionString(_setting.KanbanHost.Host);;
        kanbanSettings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
        var client = new MongoClient(kanbanSettings);
        _clients = new List<IMongoClient> { client };
        _repository = new MongoRepository(_clients);

        this.worker = new KanbanDatabaseWorker(_repository, _setting);

        this.Dispose();
    }

    public override void Before(MethodInfo methodUnderTest)
    {
        this.Migrate();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        this.Dispose();
    }

    private void Migrate()
    {
        IMongoCollection<CardDto> collection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<CardDto>(_setting.Collections.Cards);
        var cardOne = JsonConvert.DeserializeObject<CardDto>(CardMocks.SampleMockOne);
        var cardTwo = JsonConvert.DeserializeObject<CardDto>(CardMocks.SampleMockTwo);
        var cardThree = JsonConvert.DeserializeObject<CardDto>(CardMocks.SampleMockThree);
        var list = new List<CardDto> { cardOne, cardTwo, cardThree };
        collection.InsertMany(list);
    }

    private void Dispose()
    {
        IMongoCollection<CardDto> collection = _clients.ElementAt(_setting.KanbanHost.ClusterId).GetDatabase(_setting.KanbanHost.Database).GetCollection<CardDto>(_setting.Collections.Cards);
        collection.DeleteMany(Builders<CardDto>.Filter.Empty);
    }
}
