using Kanban.Model.Dto.Application.Column;

namespace Kanban.Application.Interfaces;

public interface IColumnService
{
    public Task<ColumnActionResponse> AddColumn(AddColumnRequest request);
    Task<ColumnActionResponse> UpdateColumn(UpdateColumnRequest request);
}
