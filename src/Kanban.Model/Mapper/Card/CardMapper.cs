using Api = Kanban.Model.Dto.API.Card;
using App = Kanban.Model.Dto.Application.Card;
using Repo = Kanban.Model.Dto.Repository.Card;

namespace Kanban.Model.Mapper.Card;

public static class CardMapper
{
    public static Api.GetCardResponseDto ToPresentationGetResponse(this App.CardDto appCard)
    {   
        return new Api.GetCardResponseDto()
        {
            Cards = new List<Api.CardDto> { appCard.ToPresentationDto() }
        };
    }

    public static Api.GetCardResponseDto ToPresentationGetResponse(this List<App.CardDto> appCards)
    {
        var response = new Api.GetCardResponseDto()
        {
            Cards = appCards.ToPresentationDto(),
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

    public static List<Api.CardDto> ToPresentationDto(this List<App.CardDto> appCards)
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

    public static Api.CreateCardResponseDto ToPresentationCreateResponse(this App.CardDto card)
    {
        return new Api.CreateCardResponseDto
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

    public static List<App.CardDto> ToApplicationDto(this List<Repo.CardDto> cards)
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
