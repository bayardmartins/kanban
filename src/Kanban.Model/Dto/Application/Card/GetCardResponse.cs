namespace Kanban.Model.Dto.Application.Card;

public class GetCardResponse
{
    public List<CardDto> CardList { get; set; }
    public string Error { get; set; }
}
