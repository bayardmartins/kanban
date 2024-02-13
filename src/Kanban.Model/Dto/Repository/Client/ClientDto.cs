using MongoDB.Bson.Serialization.Attributes;

namespace Kanban.Model.Dto.Repository.Client;

public class ClientDto
{
    [BsonId]
    public string _id { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}
