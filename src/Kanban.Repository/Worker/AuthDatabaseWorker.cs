using Kanban.Repository.Interfaces;
using Kanban.Repository.Dto.Models;
using Kanban.Repository.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Kanban.CrossCutting;

namespace Kanban.Repository.Worker;

public class AuthDatabaseWorker : IAuthDatabaseWorker
{
    private readonly IMongoRepository _clientRepository;
    private readonly IMongoSettings _mongoSettings;

    public AuthDatabaseWorker(IMongoRepository clientRepository, IMongoSettings mongoSettings)
    {
        _clientRepository = clientRepository;
        _mongoSettings = mongoSettings;
    }

    public async Task<ClientDto?> GetClientById(string id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, id);

        var card = await _clientRepository.FindOne(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Clients, filter).ConfigureAwait(false);

        return card == null ? null : BsonSerializer.Deserialize<ClientDto>(card.ToJson());
    }

    public async Task RegisterClient(ClientDto client)
    {
        await _clientRepository.Insert(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Clients, client.ToBsonDocument());
    }
}
