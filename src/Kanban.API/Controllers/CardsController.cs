using Kanban.Model.Dto.API.Card;
using Kanban.Model.Mapper.Card;
using Kanban.CrossCutting;

namespace Kanban.API.Controllers;

[ApiController]
[Authorize]
[Route("boards/{boardId}/column/{columnId}/cards")]
public class CardsController : ControllerBase
{
    private readonly ILogger<CardsController> _logger;
    private readonly ICardService _cardService;

    public CardsController(ILogger<CardsController> logger, ICardService cardService)
    {
        _logger = logger;
        _cardService = cardService;
    }

    [HttpGet, CustomAuthentication]
    public async Task<ActionResult<GetCardResponse>> GetAllCards([FromRoute] string boardId, string columnId)
    {
        this.Log(nameof(GetAllCards),"Start", null);
        var cards = await _cardService.GetCards();
        this.Log(nameof(GetAllCards),"Result", cards);
        return new OkObjectResult(cards.ToPresentationGet());
    }

    [HttpGet("{id}"), CustomAuthentication]
    public async Task<ActionResult<GetCardResponse>> GetCard([FromRoute] string boardId, string columnId, string id)
    {
        this.Log(nameof(GetCard), "Start",id);
        var card = await _cardService.GetCardById(id);
        this.Log(nameof(GetCard),"Result", card);
        if(card is null)
            return new NotFoundResult();
        return new OkObjectResult(card.ToPresentationGet());
    }

    [HttpPost, CustomAuthentication]
    public async Task<ActionResult<CreateCardResponse>> CreateCard([FromRoute] string boardId, string columnId, CreateCardRequest cardRequest)
    {
        this.Log(nameof(CreateCard), "Start", cardRequest);
        var createdCard = await _cardService.CreateCard(cardRequest.ToApplication());
        this.Log(nameof(CreateCard), "Result", createdCard);
        return new OkObjectResult(createdCard.ToPresentationCreate());
    }

    [HttpDelete("{id}"), CustomAuthentication]
    public async Task<ActionResult> DeleteCard([FromRoute] string boardId, string columnId, string id)
    {
        this.Log(nameof(CreateCard), "Start", id);
        var result = await _cardService.DeleteCard(id);
        this.Log(nameof(CreateCard), "Result", new { deleted = result });
        if (result)
            return new OkObjectResult(Constants.CardDeleted);
        return new NotFoundObjectResult(Constants.CardNotFound);
    }

    [HttpPut, CustomAuthentication]
    public async Task<ActionResult> UpdateCard([FromRoute] string boardId, string columnId, UpdateCardRequest cardRequest)
    {
        this.Log(nameof(UpdateCard), "Start", cardRequest.Card);
        var result = await _cardService.UpdateCard(cardRequest.Card.ToApplication());
        this.Log(nameof(UpdateCard), "Result", result);
        if (result is not null)
            return new OkObjectResult(Constants.CardUpdated);
        return new NotFoundObjectResult(Constants.CardNotFound);
    }

    private void Log(string methodName, string stage, object? data)
    {
        this._logger.LogInformation($"{methodName}: {stage}", () => new { data });
    }
}