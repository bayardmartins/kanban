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
        for (int i = 0; i <= appColumns.Length; i++)
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
}
