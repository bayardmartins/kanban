using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Kanban.Repository.Dto.Models;

public class CardDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
