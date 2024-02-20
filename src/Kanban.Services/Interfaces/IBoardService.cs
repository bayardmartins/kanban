using Kanban.Model.Dto.Application.Board;

namespace Kanban.Application.Interfaces;

public interface IBoardService
{
    public Task<BoardDto?> GetBoard(string boardId);
    public Task<BoardDto> CreateBoard(BoardDto board);
    public Task<BoardDto?> UpdateBoard(BoardDto board);
    public Task<BoardActionResult> DeleteBoard(string id);
    public Task<List<BoardDto?>> GetAllBoards();
}
