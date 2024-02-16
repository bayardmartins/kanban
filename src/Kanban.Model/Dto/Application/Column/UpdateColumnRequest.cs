namespace Kanban.Model.Dto.Application.Column;

public class UpdateColumnRequest
{
    public string ColumnId { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
    public string BoardId { get; set; } = string.Empty;
}
