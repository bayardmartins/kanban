using Kanban.Model.Dto.Repository.Board;
using Kanban.Model.Dto.Repository.Column;

namespace Kanban.Repository.Interfaces;

public interface IBoardsDatabaseWorker
{
    public Task<BoardDto?> GetBoardById(string id);
    public Task<BoardDto> InsertBoard(BoardDto board);
    public Task<BoardDto?> UpdateBoard(BoardDto board);
    public Task<bool> DeleteById(string id);
    public Task<string?> UpdateBoardColumns(BoardDto board, int index, bool newColumn = true);
    public Task<bool?> UpdateBoardColumnName(UpdateColumnRequest updateColumnRequest);
    public Task<bool?> DeleteColumn(string boardId, string columnId);
    public Task<bool?> UpdateColumnCards(string boardId, ColumnDto column);
}
