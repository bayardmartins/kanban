﻿using Kanban.Application.Interfaces;
using Kanban.Model.Dto.Application.Board;
using Kanban.Model.Mapper.Board;
using Kanban.Repository.Interfaces;

namespace Kanban.Application.Services;

public class ColumnService : IColumnService
{
    private readonly IBoardsDatabaseWorker _boardDatabaseWorker;

    public ColumnService(IBoardsDatabaseWorker boardDatabaseWorker)
    {
        _boardDatabaseWorker = boardDatabaseWorker;
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