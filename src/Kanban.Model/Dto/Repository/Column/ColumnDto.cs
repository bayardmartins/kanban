using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Kanban.Model.Dto.Repository.Column;

public class ColumnDto
{
    public string _id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string[] Cards { get; set; } = new string[0];
}
