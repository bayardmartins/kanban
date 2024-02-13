using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Kanban.Model.Dto.Repository.Card;

public class CardDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
