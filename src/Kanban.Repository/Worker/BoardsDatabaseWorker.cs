using Amazon.Runtime.Internal;
using Kanban.CrossCutting;
using Kanban.Model.Dto.Repository.Board;
using Kanban.Model.Dto.Repository.Column;
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

    public async Task<BoardDto> InsertBoard(BoardDto board)
    {
        board._id = new ObjectId().ToString();
        var document = await _boardRepository.Insert(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Boards, board.ToBsonDocument());
        return BsonSerializer.Deserialize<BoardDto>(document.ToJson());
    }

    public async Task<BoardDto?> UpdateBoard(BoardDto board)
    {
        var validId = ObjectId.TryParse(board._id, out var parsedId);
        if (!validId)
            return null;
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parsedId);
        var update = Builders<BsonDocument>.Update
                    .Set(Constants.Name, board.Name);
        var options = new UpdateOptions
        {
            IsUpsert = false
        };
        var response = await _boardRepository.Update(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Boards, filter, update, options);
        return response.ModifiedCount > 0 ? board : null;
    }

    public async Task<bool> DeleteById(string id)
    {
        var validId = ObjectId.TryParse(id, out var parsedId);
        if (!validId)
            return false;
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parsedId);
        var result = await _boardRepository.Delete(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Boards, filter);
        return result.DeletedCount == 1;
    }

    public async Task<string?> UpdateBoardColumns(BoardDto board, int index)
    {
        var validId = ObjectId.TryParse(board._id, out var parsedId);
        if (!validId)
            return null;
        board.Columns[index]._id = Guid.NewGuid().ToString();
        var filter = Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parsedId);
        var update = Builders<BsonDocument>.Update
                    .Set(Constants.Columns, board.Columns);
        var options = new UpdateOptions
        {
            IsUpsert = false
        };
        var response = await _boardRepository.Update(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Boards, filter, update, options);
        return response.ModifiedCount == 1 ? board.Columns[index]._id : string.Empty;
    }

    public async Task<bool?> UpdateBoardColumnName(UpdateColumnRequest request)
    {
        var validBoardId = ObjectId.TryParse(request.BoardId, out var parseBoarddId);
        if (!validBoardId)
            return null;

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parseBoarddId),
            Builders<BsonDocument>.Filter.Eq("Columns._id", request.ColumnId));

        var update = Builders<BsonDocument>.Update.Set("Columns.$.Name", request.ColumnName);

        var options = new UpdateOptions
        {
            IsUpsert = false
        };
        var response = await _boardRepository.Update(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Boards, filter, update, options);
        return response.ModifiedCount > 0;
    }

    public async Task<bool?> DeleteColumn(string boardId, string columnId)
    {
        var validBoardId = ObjectId.TryParse(boardId, out var parseBoarddId);
        if (!validBoardId)
            return null;
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq(Constants.MongoDbId, parseBoarddId),
            Builders<BsonDocument>.Filter.Eq("Columns._id", columnId));

        var update = Builders<BsonDocument>.Update.PullFilter(Constants.Columns, Builders<ColumnDto>.Filter.Eq(Constants.MongoDbId, columnId));

        var options = new UpdateOptions
        {
            IsUpsert = false
        };

        var response = await _boardRepository.Update(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Boards, filter, update, options);
        return response.ModifiedCount == 1;
    }
}
