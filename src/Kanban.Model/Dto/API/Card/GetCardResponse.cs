namespace Kanban.Model.Dto.API.Card;

public class GetCardResponse
{
    public List<CardDto> Cards {  get; set; } = new List<CardDto>();
    public string Error { get; set; }
}
