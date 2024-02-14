using Kanban.Model.Dto.API.Board;
using System.Data.Common;
using System.Net.NetworkInformation;
using Api = Kanban.Model.Dto.API.Board;
using App = Kanban.Model.Dto.Application.Board;
using Repo = Kanban.Model.Dto.Repository.Board;
namespace Kanban.Model.Mapper.Board;

public static class BoardMapper
{
    public static App.BoardDto ToApplication(this Repo.BoardDto board)
    {
        return new App.BoardDto
        {
            Id = board._id,
            Name = board.Name,
            Columns = board.Columns.ToApplication(),
        };
    }

    public static App.ColumnDto[] ToApplication(this Repo.ColumnDto[] columns)
    {
        var appColumns = new App.ColumnDto[columns.Length];
        for (int i = 0; i < appColumns.Length; i++)
            appColumns[i] = columns[i].ToApplication();
        return appColumns;
    }

    public static App.ColumnDto ToApplication(this Repo.ColumnDto column)
    {
        return new App.ColumnDto
        {
            Id = column._id,
            Name = column.Name,
            Cards = column.Cards
        };
    }

    public static Api.BoardDto ToPresentation(this App.BoardDto board)
    {
        return new Api.BoardDto
        {
            Id = board.Id,
            Name = board.Name,
            Columns = board.Columns.ToPresentation(),
        };
    }

    public static Api.ColumnDto[] ToPresentation(this App.ColumnDto[] columns)
    {
        var apiColumns = new Api.ColumnDto[columns.Length];
        for (int i = 0; i < apiColumns.Length; i++)
            apiColumns[i] = columns[i].ToPresentation();
        return apiColumns;
    }

    public static Api.ColumnDto ToPresentation(this App.ColumnDto column)
    {
        return new Api.ColumnDto
        {
            Id = column.Id,
            Name = column.Name,
            Cards = column.Cards
        };
    }

    public static Api.GetBoardResponse ToPresentationGet(this App.BoardDto board)
    {
        return new GetBoardResponse
        {
            Board = board.ToPresentation(),
        };
    }

    public static App.BoardDto ToApplication(this Api.CreateBoardRequest request)
    {
        return new App.BoardDto
        {
            Name = request.Name,
        };
    }

    public static Repo.BoardDto ToDatabase(this App.BoardDto board)
    {
        return new Repo.BoardDto
        {
            _id = board.Id,
            Name = board.Name,
            Columns = board.Columns.ToDatabase(),
        };
    }

    public static Repo.ColumnDto[] ToDatabase(this App.ColumnDto[] columns)
    {
        var repoColumn = new Repo.ColumnDto[columns.Length];
        for (int i = 0; i < repoColumn.Length; i++)
            repoColumn[i] = columns[i].ToDatabase();
        return repoColumn;
    }

    public static Repo.ColumnDto ToDatabase(this App.ColumnDto column)
    {
        return new Repo.ColumnDto
        {
            _id = column.Id,
            Name = column.Name,
            Cards = column.Cards
        };
    }

    public static Api.CreateBoardResponse ToPresentationCreate(this App.BoardDto board)
    {
        return new Api.CreateBoardResponse
        {
            Board = board.ToPresentation(),
        };
    }
}
