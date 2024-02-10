using MongoDB.Bson.Serialization.Attributes;

namespace Kanban.Repository.Dto.Models;

public class ClientDto
{
    [BsonId]
    public string Id { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}
