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

    public async Task<bool> DeleteBoard(string id)
    {
        return await this._boardDatabaseWorker.DeleteById(id);
    }

    public async Task<ColumnUpdateResponse> AddColumn(ColumnAddRequest request)
    {
        var response = new ColumnUpdateResponse();
        var board = await this._boardDatabaseWorker.GetBoardById(request.BoardId);
        if (board == null)
        {
            response.Error = "Board not found";
            return response;
        }
        if (request.Index > board.Columns.Length)
        {
            response.Error = "Index out of boundary";
            return response;
        }
        var appBoard = board.ToApplication();
        appBoard.Columns.Insert(request.Index, request.Column);
        var successUpdade = await this._boardDatabaseWorker.UpdateBoardColumns(appBoard.ToDatabase(), request.Index);
        if (!successUpdade)
        {
            response.Error = "Invalid board id";
            return response;
        }
        return response;
    }
}
