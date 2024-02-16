using Kanban.Application.Interfaces;
using Kanban.Model.Dto.Application.Board;
using Kanban.Repository.Interfaces;
using Kanban.Model.Mapper.Board;

namespace Kanban.Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardsDatabaseWorker _boardDatabaseWorker;
    private readonly ICardsDatabaseWorker _cardDatabaseWorker;

    public BoardService(IBoardsDatabaseWorker boardsDatabaseWorker, ICardsDatabaseWorker cardDatabaseWorker)
    {
        _boardDatabaseWorker = boardsDatabaseWorker;
        _cardDatabaseWorker = cardDatabaseWorker;
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

    public async Task<BoardActionResult> DeleteBoard(string id)
    {
        var result = new BoardActionResult();
        var board = await this._boardDatabaseWorker.GetBoardById(id);
        if (board == null)
        {
            result.Error = "BoardId invalid";
            return result;
        }
        if (board.Columns.Length > 0)
        {
            result.Error = $"Board with columns can't be deleted. Board has {board.Columns.Length} columns. Delete all columns before deleting board";
        }
        var response = await this._boardDatabaseWorker.DeleteById(id);
        if (response)
        {
            result.BoardId = id;
            return result;
        }
        result.Error = "Failed to delete board";
        return result;
    }
}
