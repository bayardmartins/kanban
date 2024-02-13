using Kanban.Model.Dto.Repository.Board;

namespace Kanban.Repository.Interfaces;

public interface IBoardsDatabaseWorker
{
    public Task<BoardDto> GetBoardById(string id);
}
