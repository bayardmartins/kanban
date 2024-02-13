using Kanban.Application.Interfaces;
using Kanban.Model.Dto.Application.Board;
using Kanban.Repository.Interfaces;
using Kanban.Model.Mapper.Board;

namespace Kanban.Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardsDatabaseWorker _boardDatabaseWorker;

    public BoardService(IBoardsDatabaseWorker boardsDatabaseWorker)
    {
        _boardDatabaseWorker = boardsDatabaseWorker;
    }

    public async Task<BoardDto?> GetBoard(string boardId)
    {
        var board = await this._boardDatabaseWorker.GetBoardById(boardId);
        return board?.ToApplication();
    }
}
