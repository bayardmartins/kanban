using MongoDB.Bson;
using MongoDB.Driver;

namespace Kanban.Repository.Interfaces;

public interface IMongoRepository
{
    Task<BsonDocument> FindOne(int host, string database, string collectionName,
        FilterDefinition<BsonDocument> filter);

    Task<List<BsonDocument>> FindMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter);

    Task<BsonDocument> Insert(int host, string database, string collectionName, BsonDocument document);

    Task<UpdateResult> Update(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, UpdateOptions options);

    Task<UpdateResult> UpdateMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, UpdateOptions options);

    Task<DeleteResult> Delete(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter);

    Task<DeleteResult> DeleteMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter);
}
