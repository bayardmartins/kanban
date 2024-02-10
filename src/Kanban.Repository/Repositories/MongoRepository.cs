using Kanban.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Kanban.Repository.Repositories;

public class MongoRepository : IMongoRepository
{
    private readonly IEnumerable<IMongoClient> _mongoClients;

    public MongoRepository(IEnumerable<IMongoClient> mongoClients)
    {
        _mongoClients = mongoClients;
    }

    public async Task<BsonDocument> FindOne(int host, string database, string collectionName, FilterDefinition<BsonDocument>? filter = null)
    {
        IMongoCollection<BsonDocument> collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);


        return await collection.Find(filter ?? FilterDefinition<BsonDocument>.Empty)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
    }

    public async Task<List<BsonDocument>> FindMany(int host, string database, string collectionName, FilterDefinition<BsonDocument>? filter = null)
    {
        IMongoCollection<BsonDocument> collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

        return await collection.Find(filter ?? FilterDefinition<BsonDocument>.Empty)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<BsonDocument> Insert(int host, string database, string collectionName, BsonDocument document)
    {
        IMongoCollection<BsonDocument> collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

        await collection.InsertOneAsync(document)
        .ConfigureAwait(false);

        return document;
    }

    public async Task<UpdateResult> Update(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, UpdateOptions options)
    {
        IMongoCollection<BsonDocument> collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

        return await collection.UpdateOneAsync(filter, update, options)
            .ConfigureAwait(false);
    }

    public async Task<UpdateResult> UpdateMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, UpdateOptions options)
    {
        IMongoCollection<BsonDocument> collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

        return await collection.UpdateManyAsync(filter, update, options)
            .ConfigureAwait(false);
    }

    public async Task<DeleteResult> Delete(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter)
    {
        IMongoCollection<BsonDocument> collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

        return await collection.DeleteOneAsync(filter)
            .ConfigureAwait(false);
    }
}
