using System.ComponentModel.DataAnnotations.Schema;
using Kanban.Model.Dto.API.Column;

namespace Kanban.Model.Dto.API.Board;

public class BoardDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<ColumnDto> Columns { get; set; } = new List<ColumnDto>();
}
