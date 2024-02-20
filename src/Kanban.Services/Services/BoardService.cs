using Kanban.Application.Interfaces;
using Kanban.Model.Dto.Application.Board;
using Kanban.Repository.Interfaces;
using Kanban.Model.Mapper.Board;
using Kanban.CrossCutting;

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

    public async Task<List<BoardDto>> GetAllBoards()
    {
        var boards = await this._boardDatabaseWorker.GetAllBoards();
        return boards.ToApplication();
    }

    public async Task<BoardDto> CreateBoard(BoardDto board)
    {
        var repoBoard = await this._boardDatabaseWorker.InsertBoard(board.ToDatabase());
        return repoBoard.ToApplication();
    }

    public async Task<BoardDto?> UpdateBoard(BoardDto board)
    {
        var result = await this._boardDatabaseWorker.UpdateBoard(board.ToDatabase());
        return result?.ToApplication();
    }

    public async Task<BoardActionResult> DeleteBoard(string id)
    {
        var result = new BoardActionResult();
        var board = await this._boardDatabaseWorker.GetBoardById(id);
        if (board == null)
        {
            result.Error = Constants.BoardInvalid;
            return result;
        }
        if (board.Columns.Length > 0)
        {
            result.Error = string.Format(Constants.BoardWithColumns, board.Columns.Length);
        }
        var response = await this._boardDatabaseWorker.DeleteById(id);
        if (response)
        {
            result.BoardId = id;
            return result;
        }
        result.Error = Constants.FailedToDeleteBoard;
        return result;
    }
}
