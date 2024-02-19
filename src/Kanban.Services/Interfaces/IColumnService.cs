using Kanban.Model.Dto.Application.Column;

namespace Kanban.Application.Interfaces;

public interface IColumnService
{
    public Task<ColumnActionResponse> AddColumn(AddColumnRequest request);
    public Task<ColumnActionResponse> DeleteColumn(string boardId, string columnId);
    public Task<ColumnActionResponse> MoveColumn(string boardId, string id, int index);
    public Task<ColumnActionResponse> UpdateColumn(UpdateColumnRequest request);
}
