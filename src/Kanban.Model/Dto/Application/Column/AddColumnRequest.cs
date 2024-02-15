namespace Kanban.Model.Dto.Application.Column;

public class AddColumnRequest
{
    public string BoardId { get; set; }
    public ColumnDto Column { get; set; }
    public int Index { get; set; }
}
