namespace Kanban.Model.Dto.Application.Board;

public class ColumnDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string[] Cards { get; set; } = new string[0];
}
