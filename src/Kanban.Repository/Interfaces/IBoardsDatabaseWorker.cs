using Kanban.Model.Dto.Repository.Board;

namespace Kanban.Repository.Interfaces;

public interface IBoardsDatabaseWorker
{
    public Task<BoardDto?> GetBoardById(string id);
    public Task<BoardDto> InsertBoard(BoardDto board);
    public Task<BoardDto?> UpdateBoard(BoardDto board);
}
