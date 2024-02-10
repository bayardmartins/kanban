namespace Kanban.Repository.Settings;

public interface IMongoSettings
{
    MongoHost KanbanHost { get; set; }

    Collections Collections { get; set; }
}

public class MongoSettings : IMongoSettings
{
    public MongoHost KanbanHost { get; set; }

    public Collections Collections { get; set; }
}

public class MongoHost
{
    public string Host { get; set; }

    public string Database { get; set; }

    public int ClusterId { get; set; }
}

public class Collections
{
    public string Cards { get; set; }
}