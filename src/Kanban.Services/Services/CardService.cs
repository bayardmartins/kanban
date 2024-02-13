using Kanban.Application.Interfaces;
using Kanban.Model.Mapper.Card;
using Kanban.Model.Dto.Application.Card;
using Kanban.Repository.Interfaces;

namespace Kanban.Application.Services;
public class CardService : ICardService
{
    private readonly ICardsDatabaseWorker _kanbanDatabaseWorker;

    public CardService(ICardsDatabaseWorker kanbanDatabaseWorker)
    {
        this._kanbanDatabaseWorker = kanbanDatabaseWorker;
    }

    public async Task<CardDto?> GetCardById(string id)
    {
        var card = await this._kanbanDatabaseWorker.GetCardById(id);
        return card?.ToApplication();
    }

    public async Task<List<CardDto>> GetCards()
    {
        var cards = await this._kanbanDatabaseWorker.GetAllCards();
        return cards is null ? new List<CardDto>() : cards.ToApplicationDto();
    }

    public async Task<CardDto> CreateCard(CardDto card)
    {
        var createdCard = await this._kanbanDatabaseWorker.InsertCard(card.ToDatabaseInsert());
        return createdCard.ToApplication();
    }

    public async Task<bool> DeleteCard(string id)
    {
        return await this._kanbanDatabaseWorker.DeleteById(id);
    }

    public async Task<CardDto?> UpdateCard(CardDto card)
    {
        var result = await this._kanbanDatabaseWorker.UpdateCard(card.ToDatabaseUpdate());
        return result?.ToApplication();
    }
}