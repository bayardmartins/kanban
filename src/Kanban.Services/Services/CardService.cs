using Kanban.Application.Interfaces;
using Kanban.Model.Mapper.Card;
using Kanban.Model.Dto.Application.Card;
using Kanban.Repository.Interfaces;

namespace Kanban.Application.Services;
public class CardService : ICardService
{
    private readonly ICardsDatabaseWorker _cardsDatabaseWorker;

    public CardService(ICardsDatabaseWorker kanbanDatabaseWorker)
    {
        this._cardsDatabaseWorker = kanbanDatabaseWorker;
    }

    public async Task<CardDto?> GetCardById(string id)
    {
        var card = await this._cardsDatabaseWorker.GetCardById(id);
        return card?.ToApplication();
    }

    public async Task<List<CardDto>> GetCards()
    {
        var cards = await this._cardsDatabaseWorker.GetAllCards();
        return cards is null ? new List<CardDto>() : cards.ToApplication();
    }

    public async Task<CardDto> CreateCard(CardDto card)
    {
        var createdCard = await this._cardsDatabaseWorker.InsertCard(card.ToDatabaseInsert());
        return createdCard.ToApplication();
    }

    public async Task<bool> DeleteCard(string id)
    {
        return await this._cardsDatabaseWorker.DeleteById(id);
    }

    public async Task<CardDto?> UpdateCard(CardDto card)
    {
        var result = await this._cardsDatabaseWorker.UpdateCard(card.ToDatabaseUpdate());
        return result?.ToApplication();
    }
}