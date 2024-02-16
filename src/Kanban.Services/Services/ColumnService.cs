using Kanban.Application.Interfaces;
using Kanban.Model.Dto.Application.Column;
using Kanban.Model.Mapper.Board;
using Kanban.Model.Mapper.Column;
using Kanban.Repository.Interfaces;

namespace Kanban.Application.Services;

public class ColumnService : IColumnService
{
    private readonly IBoardsDatabaseWorker _boardDatabaseWorker;

    public ColumnService(IBoardsDatabaseWorker boardDatabaseWorker)
    {
        _boardDatabaseWorker = boardDatabaseWorker;
    }

    public async Task<ColumnActionResponse> AddColumn(AddColumnRequest request)
    {
        var response = new ColumnActionResponse();
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
        var update = await this._boardDatabaseWorker.UpdateBoardColumns(appBoard.ToDatabase(), request.Index);
        if (update is null)
        {
            response.Error = "Invalid board id";
            return response;
        }
        else if (string.Empty.Equals(update))
        {
            response.Error = "Failed to update";
            return response;
        }
        response.ColumnId = update;
        return response;
    }

    public async Task<ColumnActionResponse> UpdateColumn(UpdateColumnRequest request)
    {
        var response = new ColumnActionResponse();
        var board = await this._boardDatabaseWorker.GetBoardById(request.BoardId);
        if (board == null)
        {
            response.Error = "Board not found";
            return response;
        }
        if (board.Columns.FirstOrDefault(x => x._id == request.ColumnId) == null)
        {
            response.Error = "Column not found";
            return response;
        }

        var res = await this._boardDatabaseWorker.UpdateBoardColumnName(request.ToDatabase());

        switch (res) 
        {
            case false: response.Error = "Column not found"; break;
            case null: response.Error = "Invalid Id"; break;
            case true: break;
        }

        return response;
    }

    public async Task<ColumnActionResponse> DeleteColumn(string boardId, string columnId)
    {
        var response = new ColumnActionResponse();
        var board = await this._boardDatabaseWorker.GetBoardById(boardId);
        if (board == null)
        {
            response.Error = "Board not found";
            return response;
        }
        var column = board.Columns.FirstOrDefault(x => x._id == columnId);
        if (column == null)
        {
            response.Error = "Column not found";
            return response;
        }
        if (column.Cards.Length > 0)
        {
            response.Error = $"Column with cards can't be deleted. Column has {column.Cards.Length} cards. Delete all cards before deleting column";
            return response;
        }

        bool? res = await this._boardDatabaseWorker.DeleteColumn(boardId, columnId);
        if (res is not null && res == true)
        {
            response.ColumnId = columnId;
            return response;
        }
        else if (res is null)
        {
            response.Error = "BoardId invalid";
            return response;
        }
        response.Error = "Failed to delete column";
        return response;
    }
}
