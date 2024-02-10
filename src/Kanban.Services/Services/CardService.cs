using Kanban.Application.Interfaces;
using Kanban.Application.Dto.Mapper;
using Kanban.Application.Dto.Models;
using Kanban.Repository.Interfaces;

namespace Kanban.Application.Services;
public class CardService : ICardService
{
    private readonly IKanbanDatabaseWorker _kanbanDatabaseWorker;

    public CardService(IKanbanDatabaseWorker kanbanDatabaseWorker)
    {
        this._kanbanDatabaseWorker = kanbanDatabaseWorker;
    }

    public async Task<CardDto> GetCard(string id)
    {
        var card = await this._kanbanDatabaseWorker.GetCardById(id);
        return card is null ? new CardDto() : card.ToApplication();
    }

    public async Task<List<CardDto>> GetCards()
    {
        var cards = await this._kanbanDatabaseWorker.GetAllCards();
        return cards is null ? new List<CardDto>() : cards.ToApplicationDto();
    }
}