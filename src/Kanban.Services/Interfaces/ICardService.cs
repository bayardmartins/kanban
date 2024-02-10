using Kanban.Application.Dto.Models;

namespace Kanban.Application.Interfaces;

public interface ICardService
{
    public Task<CardDto> GetCard(string id);
    public Task<List<CardDto>> GetCards();
}
