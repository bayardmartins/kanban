using Kanban.Repository.Interfaces;
using Kanban.Repository.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Kanban.CrossCutting;
using Kanban.Model.Dto.Repository.Card;

namespace Kanban.Repository.Worker;

public class CardsDatabaseWorker : ICardsDatabaseWorker
{

    private readonly IMongoRepository _cardRepository;
    private readonly IMongoSettings _mongoSettings;
    public CardsDatabaseWorker(IMongoRepository cardRepository, IMongoSettings mongoSettings)
    {
        this._cardRepository = cardRepository;
        this._mongoSettings = mongoSettings;
    }

    public async Task<CardDto?> GetCardById(string id)
    {
        var validId = ObjectId.TryParse(id, out var parsedId);
        if (!validId)
            return null;
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parsedId);

        var card = await _cardRepository.FindOne(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filter).ConfigureAwait(false);

        return card == null ? null : BsonSerializer.Deserialize<CardDto>(card.ToJson());
    }

    public async Task<List<CardDto>?> GetAllCards(string[] cardList)
    {
        var validList = cardList.ToList().ConvertAll(x => ObjectId.TryParse(x, out _));
        if (validList.Any(x => x.Equals(false)))
            return null;

        var filter = Builders<BsonDocument>.Filter.In(Constants.MongoDbId, cardList.ToList().ConvertAll(x => ObjectId.Parse(x)));

        var cards = await _cardRepository.FindMany(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filter).ConfigureAwait(false);

        return BsonSerializer.Deserialize<List<CardDto>>(cards.ToJson());
    }

    public async Task<CardDto> InsertCard(CardDto card)
    {
        card._id = new ObjectId().ToString();
        var document = await _cardRepository.Insert(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, card.ToBsonDocument());
        return BsonSerializer.Deserialize<CardDto>(document);
    }

    public async Task<CardDto?> UpdateCard(CardDto card)
    {
        var validId = ObjectId.TryParse(card._id, out var parsedId);
        if (!validId)
            return null;
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parsedId);
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
        var validList = ids.ConvertAll(x => ObjectId.TryParse(x, out _));
        if (validList.Any(x => x.Equals(false)))
            return 0;
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
        var validId = ObjectId.TryParse(id, out var parsedId);
        if (!validId)
            return false;
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parsedId);
        var result = await _cardRepository.Delete(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filter);
        return result.DeletedCount == 1;
    }

    public async Task<bool> DeleteMany(List<string> ids)
    {
        var validList = ids.ConvertAll(x => ObjectId.TryParse(x, out _));
        if (validList.Any(x => x.Equals(false)))
            return false;
        var filter = Builders<BsonDocument>.Filter.In(Constants.MongoDbId, ids.ConvertAll(x => ObjectId.Parse(x)));
        var result = await _cardRepository.DeleteMany(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, filter);
        return result.DeletedCount == ids.Count;
    }
}
