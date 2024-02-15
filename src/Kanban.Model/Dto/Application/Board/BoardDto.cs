using Kanban.Model.Dto.Application.Column;

namespace Kanban.Model.Dto.Application.Board;

public class BoardDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<ColumnDto> Columns { get; set; } = new List<ColumnDto>();
}
