using Kanban.Model.Dto.Application.Board;

namespace Kanban.Application.Interfaces;

public interface IColumnService
{
    public Task<ColumnUpdateResponse> AddColumn(ColumnAddRequest request);
}
