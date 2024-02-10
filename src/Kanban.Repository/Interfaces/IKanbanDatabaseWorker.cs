using Kanban.Repository.Dto.Models;

namespace Kanban.Repository.Interfaces
{
    public interface IKanbanDatabaseWorker
    {
        public Task<CardDto?> GetCardById(string id);
        
        public Task<List<CardDto>> GetAllCards();
    }
}
