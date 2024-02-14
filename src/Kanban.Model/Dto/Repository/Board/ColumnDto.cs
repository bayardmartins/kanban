using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Kanban.Model.Dto.Repository.Board;

public class ColumnDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string[] Cards { get; set; } = new string[0];
}
