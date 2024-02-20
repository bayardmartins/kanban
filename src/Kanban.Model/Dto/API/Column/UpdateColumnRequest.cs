using System.ComponentModel.DataAnnotations;

namespace Kanban.Model.Dto.API.Column;

public class UpdateColumnRequest
{
    [Required]
    public string ColumnName { get; set; }
}
