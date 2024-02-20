using Kanban.Application.Interfaces;
using Kanban.Model.Mapper.Card;
using Kanban.Model.Dto.Application.Card;
using Kanban.Repository.Interfaces;
using System;

namespace Kanban.Application.Services;
public class CardService : ICardService
{
    private readonly ICardsDatabaseWorker _cardsDatabaseWorker;
    private readonly IBoardsDatabaseWorker _boardsDatabaseWorker;

    public CardService(ICardsDatabaseWorker kanbanDatabaseWorker, IBoardsDatabaseWorker boardsDatabaseWorker)
    {
        this._cardsDatabaseWorker = kanbanDatabaseWorker;
        this._boardsDatabaseWorker = boardsDatabaseWorker;
    }

    public async Task<CardDto?> GetCardById(string id)
    {
        var card = await this._cardsDatabaseWorker.GetCardById(id);
        return card?.ToApplication();
    }

    public async Task<GetCardResponse> GetCards(string boardId, string columnId)
    {
        var response = new GetCardResponse();
        var board = await this._boardsDatabaseWorker.GetBoardById(boardId);
        if (board is null)
        {
            response.Error = "Board not found";
            return response;
        }
        var column = board.Columns.FirstOrDefault(x => x._id == columnId);
        if (column is null)
        {
            response.Error = "Column not found";
            return response;
        }

        var cards = await this._cardsDatabaseWorker.GetAllCards(column.Cards);
        if (cards is null || cards.Count == 0)
        {
            response.Error = "Cards not found";
            return response;
        }
        return new GetCardResponse { CardList = cards.ToApplication() };
    }

    public async Task<CardDto> CreateCard(CardDto card)
    {
        var createdCard = await this._cardsDatabaseWorker.InsertCard(card.ToDatabaseInsert());
        return createdCard.ToApplication();
    }

    public async Task<CardActionResponse> DeleteCard(string boardId, string columnId, string id)
    {
        var response = new CardActionResponse();
        var board = await this._boardsDatabaseWorker.GetBoardById(boardId);
        if (board is null)
        {
            response.Error = "Board not found";
            return response;
        }
        var column = board.Columns.FirstOrDefault(x => x._id == columnId);
        if (column == null)
        {
            response.Error = "Column not found";
            return response;
        }
        var result = await this._cardsDatabaseWorker.DeleteById(id);
        if (result)
        {
            column.Cards = column.Cards.Where(cardId => cardId != id).ToArray();
            var update = await this._boardsDatabaseWorker.UpdateBoardColumns(board, 0, false);
            return response;
        }
        else
        {
            response.Error = "Unable to delete card";
            return response;
        }
    }

    public async Task<CardDto?> UpdateCard(CardDto card)
    {
        var result = await this._cardsDatabaseWorker.UpdateCard(card.ToDatabaseUpdate());
        return result?.ToApplication();
    }

    public async Task<CardActionResponse> MoveCardInPriority(string boardId, string columnId, string cardId, int index)
    {
        var response = new CardActionResponse();
        var board = await this._boardsDatabaseWorker.GetBoardById(boardId);
        if (board is null)
        {
            response.Error = "Board not found";
            return response;
        }
        var column = board.Columns.FirstOrDefault(x => x._id == columnId);
        if (column is null)
        {
            response.Error = "Column not found";
            return response;
        }
        if (column.Cards.Length <= index)
        {
            response.Error = "Index out of boundary";
            return response;
        }
        var currentIndex = Array.IndexOf(column.Cards, cardId);
        if (currentIndex == -1)
        {
            response.Error = "Card not found";
            return response;
        }

        column.Cards = column.Cards.Where((card, index) => index != currentIndex).ToArray();
        var newColumn = column.Cards.ToList();    
        newColumn.Insert(index, cardId);
        column.Cards = newColumn.ToArray();

        bool? result = await this._boardsDatabaseWorker.UpdateColumnCards(boardId, column);
        if(result is not null && result == true)
        {
            return response;
        }
        else if (result is null)
        {
            response.Error = "Invalid BoardId";
            return response;
        }
        else
        {
            response.Error = "Failed to update column";
            return response;
        }
    }

    public async Task<CardActionResponse> MoveCardInColumn(string boardId, string originColumnId, string cardId, string destinyColumnId)
    {
        var response = new CardActionResponse();
        var board = await this._boardsDatabaseWorker.GetBoardById(boardId);
        if (board is null)
        {
            response.Error = "Board not found";
            return response;
        }
        var column = board.Columns.FirstOrDefault(x => x._id == originColumnId);
        if (column is null)
        {
            response.Error = "Origin Column not found";
            return response;
        }
        var destinyColumn = board.Columns.FirstOrDefault(x => x._id == destinyColumnId);
        if (destinyColumn is null)
        {
            response.Error = "Destiny Column not found";
            return response;
        }
        if (originColumnId == destinyColumnId)
        {
            response.Error = "Origin and Destiny column are the same";
            return response;
        }
        var currentIndex = Array.IndexOf(column.Cards, cardId);
        if (currentIndex == -1)
        {
            response.Error = "Card not found";
            return response;
        }

        column.Cards = column.Cards.Where((card, index) => index != currentIndex).ToArray();
        destinyColumn.Cards = destinyColumn.Cards.Append(cardId).ToArray();

        bool? removeResult = await this._boardsDatabaseWorker.UpdateColumnCards(boardId, column);
        bool? addResult = await this._boardsDatabaseWorker.UpdateColumnCards(boardId, destinyColumn);
        if (addResult is not null && addResult == true && removeResult is not null && removeResult == true )
        {
            return response;
        }
        else if (removeResult is null || addResult is null)
        {
            response.Error = "Invalid BoardId";
            return response;
        }
        else
        {
            response.Error = "Failed to move card";
            return response;
        }
    }
}