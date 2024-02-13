using Kanban.CrossCutting;
using Kanban.Model.Dto.Repository.Board;
using Kanban.Repository.Interfaces;
using Kanban.Repository.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Kanban.Repository.Worker;

public class BoardsDatabaseWorker : IBoardsDatabaseWorker
{
    private readonly IMongoRepository _boardRepository;
    private readonly IMongoSettings _mongoSettings;

    public BoardsDatabaseWorker(IMongoRepository boardRepository, IMongoSettings mongoSettings)
    {
        this._boardRepository = boardRepository;
        this._mongoSettings = mongoSettings;
    }

    public async Task<BoardDto?> GetBoardById(string id)
    {
        var validId = ObjectId.TryParse(id, out var parsedId);
        if (!validId)
            return null;
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parsedId);

        var board = await _boardRepository.FindOne(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Boards, filter).ConfigureAwait(false);

        return board == null ? null : BsonSerializer.Deserialize<BoardDto>(board.ToJson());
    }
}
