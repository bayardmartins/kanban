using Kanban.Repository.Interfaces;
using Kanban.Repository.Dto.Models;
using Kanban.Repository.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Kanban.Repository.Worker;

public class KanbanDatabaseWorker : IKanbanDatabaseWorker
{

    private readonly IMongoRepository _cardRepository;
    private readonly IMongoSettings _mongoSettings;
    public KanbanDatabaseWorker(IMongoRepository cardRepository, IMongoSettings mongoSettings)
    {
        this._cardRepository = cardRepository;
        this._mongoSettings = mongoSettings;
    }

    public async Task<CardDto?> GetCardById(string id)
    {
        FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));

        var card = await _cardRepository.FindOne(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filterDefinition).ConfigureAwait(false);

        return card == null ? null : BsonSerializer.Deserialize<CardDto>(card.ToJson());
    }

    public async Task<List<CardDto>> GetAllCards()
    {
        var cards = await _cardRepository.FindMany(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards).ConfigureAwait(false);

        return cards == null ? new List<CardDto>() : BsonSerializer.Deserialize<List<CardDto>>(cards.ToJson());
    }
}
