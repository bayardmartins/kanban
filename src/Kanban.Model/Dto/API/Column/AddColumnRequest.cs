using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Column;

public class AddColumnRequest
{
    [Required]
    public string ColumnName { get; set; }
    [Required]
    public int Position { get; set; }
}
