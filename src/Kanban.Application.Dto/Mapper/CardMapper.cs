using Repo = Kanban.Repository.Dto.Models;
using App = Kanban.Application.Dto.Models;

namespace Kanban.Application.Dto.Mapper
{
    public static class CardMapper
    {
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
}
