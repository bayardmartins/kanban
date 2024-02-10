using Kanban.Repository.Interfaces;
using Kanban.Repository.Dto.Models;
using Kanban.Repository.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Kanban.CrossCutting;

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
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, ObjectId.Parse(id));

        var card = await _cardRepository.FindOne(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filter).ConfigureAwait(false);

        return card == null ? null : BsonSerializer.Deserialize<CardDto>(card.ToJson());
    }

    public async Task<List<CardDto>> GetAllCards()
    {
        var cards = await _cardRepository.FindMany(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards).ConfigureAwait(false);

        return BsonSerializer.Deserialize<List<CardDto>>(cards.ToJson());
    }

    public async Task<CardDto> InsertCard(CardDto card)
    {
        await _cardRepository.Insert(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, card.ToBsonDocument());
        return card;
    }

    public async Task<CardDto?> UpdateCard(CardDto card)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, ObjectId.Parse(card._id));
        var update = Builders<BsonDocument>.Update
                    .Set(Constants.Name, card.Name)
                    .Set(Constants.Description, card.Description);
        var options = new UpdateOptions
        {
            IsUpsert = false
        };
        var response = await _cardRepository.Update(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filter, update, options);
        return response.ModifiedCount > 0 ? card : null;
    }

    public async Task<long> UpdateManyDescriptions(List<string> ids, string description)
    {
        var filter = Builders<BsonDocument>.Filter.In(Constants.MongoDbId, ids.ConvertAll(x => ObjectId.Parse(x)));
        var update = Builders<BsonDocument>.Update
                    .Set(Constants.Description, description);
        var options = new UpdateOptions
        {
            IsUpsert = false
        };
        var response = await _cardRepository.UpdateMany(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filter, update, options);
        return response.ModifiedCount;
    }

    public async Task<bool> DeleteById(string id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, ObjectId.Parse(id));
        var result = await _cardRepository.Delete(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filter);
        return result.DeletedCount == 1;
    }
}
