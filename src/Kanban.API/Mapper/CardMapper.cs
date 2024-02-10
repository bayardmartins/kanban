using System.Runtime.CompilerServices;
using Api = Kanban.API.Dto.Card;
using App = Kanban.Application.Dto.Models;

namespace Kanban.API.Mapper
{
    public static class CardMapper
    {
        public static Api.CardResponseDto ToPresentationResponse(this App.CardDto appCard)
        {   
            return new Api.CardResponseDto()
            {
                cards = new List<Api.CardDto> { appCard.ToPresentationDto() }
            };
        }

        public static Api.CardResponseDto ToPresentationResponse(this List<App.CardDto> appCards)
        {
            var response = new Api.CardResponseDto()
            {
                cards = appCards.ToPresentationDto(),
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
    }
}
