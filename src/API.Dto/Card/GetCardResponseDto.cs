namespace Kanban.API.Dto.Card;

public class GetCardResponseDto
{
    public List<CardDto> Cards {  get; set; } = new List<CardDto>();
}
