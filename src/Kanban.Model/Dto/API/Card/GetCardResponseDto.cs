namespace Kanban.Model.Dto.API.Card;

public class GetCardResponseDto
{
    public List<CardDto> Cards {  get; set; } = new List<CardDto>();
}
