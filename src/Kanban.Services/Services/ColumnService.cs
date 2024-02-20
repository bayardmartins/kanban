using Kanban.Application.Interfaces;
using Kanban.CrossCutting;
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
            response.Error = Constants.BoardNotFound;
            return response;
        }
        if (request.Index > board.Columns.Length)
        {
            response.Error = Constants.OutOfBoundary;
            return response;
        }
        var appBoard = board.ToApplication();
        appBoard.Columns.Insert(request.Index, request.Column);
        var update = await this._boardDatabaseWorker.UpdateBoardColumns(appBoard.ToDatabase(), request.Index);
        if (update is null)
        {
            response.Error = Constants.BoardInvalid;
            return response;
        }
        else if (string.Empty.Equals(update))
        {
            response.Error = Constants.FailedToUpdateColumn;
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
            response.Error = Constants.BoardNotFound;
            return response;
        }
        if (board.Columns.FirstOrDefault(x => x._id == request.ColumnId) == null)
        {
            response.Error = Constants.ColumnNotFound;
            return response;
        }

        var res = await this._boardDatabaseWorker.UpdateBoardColumnName(request.ToDatabase());

        switch (res) 
        {
            case false: response.Error = Constants.ColumnNotFound; break;
            case null: response.Error = Constants.BoardInvalid; break;
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
            response.Error = Constants.BoardNotFound;
            return response;
        }
        var column = board.Columns.FirstOrDefault(x => x._id == columnId);
        if (column == null)
        {
            response.Error = Constants.ColumnNotFound;
            return response;
        }
        if (column.Cards.Length > 0)
        {
            response.Error = string.Format(Constants.ColumnWithCards,column.Cards.Length);
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
            response.Error = Constants.BoardInvalid;
            return response;
        }
        response.Error = Constants.FailedToDeleteColumn;
        return response;
    }

    public async Task<ColumnActionResponse> MoveColumn(string boardId, string id, int index)
    {
        var response = new ColumnActionResponse();
        var board = await this._boardDatabaseWorker.GetBoardById(boardId);
        if (board is null)
        {
            response.Error = Constants.BoardNotFound;
            return response;
        }
        var column = board.Columns.FirstOrDefault(x => x._id ==  id);
        if (column is null)
        {
            response.Error = Constants.ColumnNotFound;
            return response;
        }
        if(board.Columns.Length <= index) 
        {
            response.Error = Constants.OutOfBoundary;
            return response;
        }
        var currentIndex = Array.IndexOf(board.Columns, column);
        board.Columns = board.Columns.Where((column, index) => index != currentIndex).ToArray();
        var newColumns = board.Columns.ToList();
        newColumns.Insert(index, column);
        board.Columns = newColumns.ToArray();

        var result = await this._boardDatabaseWorker.UpdateBoardColumns(board, index, false);

        if (result is null)
        {
            response.Error = Constants.BoardInvalid;
            return response;
        }
        else if (string.Empty.Equals(result))
        {
            response.Error = Constants.FailedToUpdateColumn;
            return response;
        }
        else
        {
            return response;
        }
    }
}
