using Kanban.Application.Dto.Models;

namespace Kanban.Application.Interfaces;

public interface ICardService
{
    public Task<CardDto> GetCardById(string id);
    public Task<List<CardDto>> GetCards();
}
