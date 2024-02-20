using Kanban.Model.Mapper.Column;
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

    public static Api.BoardDto ToPresentation(this App.BoardDto board)
    {
        return new Api.BoardDto
        {
            Id = board.Id,
            Name = board.Name,
            Columns = board.Columns.ToPresentation(),
        };
    }


    public static Api.GetBoardResponse ToPresentationGet(this App.BoardDto board)
    {
        return new Api.GetBoardResponse
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

    public static Api.CreateBoardResponse ToPresentationCreate(this App.BoardDto board)
    {
        return new Api.CreateBoardResponse
        {
            Board = board.ToPresentation(),
        };
    }

    public static App.BoardDto ToApplicationUpdate(this Api.UpdateBoardRequest request)
    {
        return new App.BoardDto
        {
            Id = request.Id,
            Name = request.Name,
        };
    }

    public static List<App.BoardDto> ToApplication(this List<Repo.BoardDto> boards)
    {
        return boards.Select(board => board.ToApplication()).ToList();
    }

    public static List<Api.BoardDto>? ToPresentation(this List<App.BoardDto>? boards)
    {
        return boards?.Select(board => board.ToPresentation()).ToList();
    }
}
