using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Kanban.Model.Dto.Repository.Board;

public class BoardDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ColumnDto[] Columns { get; set; } = new ColumnDto[0];
}
