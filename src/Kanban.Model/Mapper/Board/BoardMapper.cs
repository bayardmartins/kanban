using Kanban.Model.Dto.API.Board;
using Api = Kanban.Model.Dto.API;
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

    public static List<App.ColumnDto> ToApplication(this Repo.ColumnDto[] columns)
    {
        return columns.Select(col => col.ToApplication()).ToList();
    }

    public static App.ColumnDto ToApplication(this Repo.ColumnDto column)
    {
        return new App.ColumnDto
        {
            Id = column._id,
            Name = column.Name,
            Cards = column.Cards.ToList(),
        };
    }

    public static Api.Board.BoardDto ToPresentation(this App.BoardDto board)
    {
        return new Api.Board.BoardDto
        {
            Id = board.Id,
            Name = board.Name,
            Columns = board.Columns.ToPresentation(),
        };
    }

    public static List<Api.Column.ColumnDto> ToPresentation(this List<App.ColumnDto> columns)
    {
        return columns.Select(col => col.ToPresentation()).ToList();
    }

    public static Api.Column.ColumnDto ToPresentation(this App.ColumnDto column)
    {
        return new Api.Column.ColumnDto
        {
            Id = column.Id,
            Name = column.Name,
            Cards = column.Cards
        };
    }

    public static Api.Board.GetBoardResponse ToPresentationGet(this App.BoardDto board)
    {
        return new GetBoardResponse
        {
            Board = board.ToPresentation(),
        };
    }

    public static App.BoardDto ToApplication(this Api.Board.CreateBoardRequest request)
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

    public static Repo.ColumnDto[] ToDatabase(this List<App.ColumnDto> columns)
    {
        return columns.ConvertAll(col => col.ToDatabase()).ToArray();
    }

    public static Repo.ColumnDto ToDatabase(this App.ColumnDto column)
    {
        return new Repo.ColumnDto
        {
            _id = column.Id,
            Name = column.Name,
            Cards = column.Cards.ToArray(),
        };
    }

    public static Api.Board.CreateBoardResponse ToPresentationCreate(this App.BoardDto board)
    {
        return new Api.Board.CreateBoardResponse
        {
            Board = board.ToPresentation(),
        };
    }

    public static App.BoardDto ToApplicationUpdate(this UpdateBoardRequest request)
    {
        return new App.BoardDto
        {
            Id = request.Id,
            Name = request.Name,
        };
    }

    public static App.ColumnAddRequest ToApplicationAdd(this Api.Column.AddColumnRequest request, string boardId)
    {
        return new App.ColumnAddRequest
        {
            BoardId = boardId,
            Index = request.Position,
            Column = new App.ColumnDto
            {
                Name = request.ColumnName,
            },
        };
    }
}
