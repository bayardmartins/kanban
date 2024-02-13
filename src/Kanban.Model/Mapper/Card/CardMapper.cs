using Kanban.Model.Dto.API.Card;
using Api = Kanban.Model.Dto.API.Card;
using App = Kanban.Model.Dto.Application.Card;
using Repo = Kanban.Model.Dto.Repository.Card;

namespace Kanban.Model.Mapper.Card;

public static class CardMapper
{
    public static Api.GetCardResponse ToPresentationGet(this App.CardDto? appCard)
    {
        return new GetCardResponse { Cards = new List<Api.CardDto> { appCard.ToPresentationDto(),} };
    }

    public static Api.GetCardResponse ToPresentationGet(this List<App.CardDto> appCards)
    {
        var response = new Api.GetCardResponse()
        {
            Cards = appCards.ToPresentation(),
        };
        return response;
    }

    public static Api.CardDto ToPresentationDto(this App.CardDto appCard)
    {
        return new Api.CardDto 
        { 
            Id = appCard.Id,
            Name = appCard.Name,
            Description = appCard.Description,
        };
    }

    public static List<Api.CardDto> ToPresentation(this List<App.CardDto> appCards)
    {
        return appCards.Select(card => card.ToPresentationDto()).ToList();
    }

    public static App.CardDto ToApplication(this Api.CardDto card, string id = "")
    {
        var appCard = new App.CardDto
        {
            Name = card.Name,
            Description = card.Description,
        };
        if (id != "")
            appCard.Id = id;
        return appCard;
    }

    public static Api.CreateCardResponse ToPresentationCreate(this App.CardDto card)
    {
        return new Api.CreateCardResponse
        {
            CreatedCard = new Api.CardDto
            {
                Id = card.Id,
                Name = card.Name,
                Description = card.Description,
            }
        };
    }

    public static App.CardDto ToApplication(this Repo.CardDto card)
    {
        App.CardDto appCard = new App.CardDto
        {
            Id = card._id,
            Name = card.Name,
            Description = card.Description,
        };

        return appCard;
    }

    public static List<App.CardDto> ToApplication(this List<Repo.CardDto> cards)
    {
        return cards.Select(card => card.ToApplication()).ToList();
    }

    public static Repo.CardDto ToDatabaseInsert(this App.CardDto card)
    {
        return new Repo.CardDto
        {
            Name = card.Name,
            Description = card.Description,
        };
    }


    public static Repo.CardDto ToDatabaseUpdate(this App.CardDto card)
    {
        return new Repo.CardDto
        {
            _id = card.Id,
            Name = card.Name,
            Description = card.Description,
        };
    }
}
