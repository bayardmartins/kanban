using Kanban.Model.Dto.Application.Card;

namespace Kanban.Application.Interfaces;

public interface ICardService
{
    public Task<CardDto?> GetCardById(string id);
    public Task<GetCardResponse> GetCards(string boardId, string columnId);
    public Task<CardDto> CreateCard(CardDto card);
    public Task<CardActionResponse> DeleteCard(string boardId, string columnId, string id);
    public Task<CardDto?> UpdateCard(CardDto card);
    public Task<CardActionResponse> MoveCardInPriority(string boardId, string id, string cardId, int index);
    public Task<CardActionResponse> MoveCardInColumn(string boardId, string originColumnId, string cardId, string destinyColumnId);
}
