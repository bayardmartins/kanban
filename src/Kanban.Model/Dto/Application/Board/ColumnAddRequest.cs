namespace Kanban.Model.Dto.Application.Board;

public class ColumnAddRequest
{
    public string BoardId { get; set; }
    public ColumnDto Column { get; set; }
    public int Index { get; set; }
}
