using Repo = Kanban.Model.Dto.Repository.Column;
using App = Kanban.Model.Dto.Application.Column;
using Api = Kanban.Model.Dto.API.Column;

namespace Kanban.Model.Mapper.Column;

public static class ColumnMapper
{
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

    public static List<Api.ColumnDto> ToPresentation(this List<App.ColumnDto> columns)
    {
        return columns.Select(col => col.ToPresentation()).ToList();
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

    public static App.AddColumnRequest ToApplication(this Api.AddColumnRequest request, string boardId)
    {
        return new App.AddColumnRequest
        {
            BoardId = boardId,
            Index = request.Position,
            Column = new App.ColumnDto
            {
                Name = request.ColumnName,
            },
        };
    }

    public static App.UpdateColumnRequest ToApplication(this Api.UpdateColumnRequest request, string boardId, string columnId)
    {
        return new App.UpdateColumnRequest
        {
            BoardId = boardId,
            ColumnId = columnId,
            ColumnName = request.ColumnName,
        };
    }

    public static Repo.UpdateColumnRequest ToDatabase(this App.UpdateColumnRequest request)
    {
        return new Repo.UpdateColumnRequest
        {
            BoardId = request.BoardId,
            ColumnId = request.ColumnId,
            ColumnName = request.ColumnName,
        };
    }
}
