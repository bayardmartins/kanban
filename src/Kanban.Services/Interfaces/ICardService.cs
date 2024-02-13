﻿using Kanban.Model.Dto.Application.Card;

namespace Kanban.Application.Interfaces;

public interface ICardService
{
    public Task<CardDto> GetCardById(string id);
    public Task<List<CardDto>> GetCards();
    public Task<CardDto> CreateCard(CardDto card);
    public Task<bool> DeleteCard(string id);
    public Task<CardDto?> UpdateCard(CardDto card);
}
