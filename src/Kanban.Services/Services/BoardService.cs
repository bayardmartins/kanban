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

    public async Task<bool> DeleteBoard(string id)
    {
        var board = await this._boardDatabaseWorker.GetBoardById(id);
        if (board == null)
        {
            return false;
        }
        var result = await this._boardDatabaseWorker.DeleteById(id);
        if (result)
        {
            var cards = board.Columns.SelectMany(column => column.Cards).ToList();
            if (cards is null || cards.Count == 0)
            {
                return true;
            }
            return await this._cardDatabaseWorker.DeleteMany(cards);
        }
        else
        {
            await this._boardDatabaseWorker.InsertBoard(board);
            return false;
        }
    }
}
