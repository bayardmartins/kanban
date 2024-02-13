using Kanban.Model.Dto.Repository.Card;

namespace Kanban.Repository.Interfaces
{
    public interface IKanbanDatabaseWorker
    {
        public Task<CardDto?> GetCardById(string id);
        
        public Task<List<CardDto>> GetAllCards();

        public Task<CardDto> InsertCard(CardDto card);

        public Task<CardDto?> UpdateCard(CardDto card);

        public Task<long> UpdateManyDescriptions(List<string> ids, string description);

        public Task<bool> DeleteById(string id);
    }
}
