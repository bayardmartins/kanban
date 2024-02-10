using MongoDB.Driver;

namespace Kanban.Repository.Infra;

public class MongoContext : IMongoContext
{
    private readonly IMongoClient mongoClient;
    private readonly IMongoDatabase database;
    private readonly List<Func<Task>> commands;

    public MongoContext(IMongoDatabase mongoDb)
    {
        this.database = mongoDb;
        this.mongoClient = mongoDb.Client;
        this.commands = new List<Func<Task>>();
    }

    public IClientSessionHandle? Session { get; set; }

    public void AddCommand(Func<Task> func)
    {
        this.commands.Add(func);
    }

    public async Task<int> SaveChanges()
    {
        using (this.Session = await this.mongoClient.StartSessionAsync())
        {
            this.Session.StartTransaction();

            var commandTasks = this.commands.Select(c => c());

            await Task.WhenAll(commandTasks);

            await this.Session.CommitTransactionAsync();
        }

        return this.commands.Count;
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return this.database.GetCollection<T>(name);
    }

    public void Dispose()
    {
        this.Session?.Dispose();
        GC.SuppressFinalize(this);
    }
}
