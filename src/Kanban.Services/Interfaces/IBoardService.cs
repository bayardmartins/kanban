using Kanban.Model.Dto.Application.Board;

namespace Kanban.Application.Interfaces;

public interface IBoardService
{
    public Task<BoardDto?> GetBoard(string boardId);
    public Task<BoardDto> CreateBoard(BoardDto board);
}
