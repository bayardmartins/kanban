using System.Runtime.CompilerServices;
using Api = Kanban.API.Dto.Card;
using App = Kanban.Application.Dto.Models;

namespace Kanban.API.Mapper
{
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

        public static App.CardDto ToApplication(this Api.CardDto card)
        {
            return new App.CardDto
            {
                Name = card.Name,
                Description = card.Description,
            };
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
    }
}
