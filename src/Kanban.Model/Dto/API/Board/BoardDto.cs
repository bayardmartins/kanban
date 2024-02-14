using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban.Model.Dto.API.Board;

public class BoardDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ColumnDto[] Columns { get; set; } = new ColumnDto[0];
}
