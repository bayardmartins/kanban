namespace Kanban.Model.Dto.API.Column;

public class ColumnDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Cards { get; set; } = new List<string>();
}
