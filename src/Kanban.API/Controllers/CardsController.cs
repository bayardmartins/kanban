using Kanban.Model.Dto.API.Card;
using Kanban.Model.Mapper.Card;
using Kanban.CrossCutting;

namespace Kanban.API.Controllers;

[ApiController]
[Authorize]
public class CardsController : ControllerBase
{
    private readonly ILogger<CardsController> _logger;
    private readonly ICardService _cardService;

    public CardsController(ILogger<CardsController> logger, ICardService cardService)
    {
        _logger = logger;
        _cardService = cardService;
    }

    [HttpGet("boards/{boardId}/column/{columnId}/cards"), CustomAuthentication]
    public async Task<ActionResult<GetCardResponse>> GetAllCards([FromRoute] string boardId, string columnId)
    {
        this.Log(nameof(GetAllCards),"Start", null);
        var getCardResponse = await _cardService.GetCards(boardId, columnId);
        this.Log(nameof(GetAllCards),"Result", getCardResponse);
        if (!string.IsNullOrEmpty(getCardResponse.Error))
            return new NotFoundObjectResult(new GetCardResponse { Error = getCardResponse.Error });
        return new OkObjectResult(getCardResponse.CardList.ToPresentationGet());
    }

    [HttpGet("cards/{id}"), CustomAuthentication]
    public async Task<ActionResult<GetCardResponse>> GetCard([FromRoute] string id)
    {
        this.Log(nameof(GetCard), "Start",id);
        var card = await _cardService.GetCardById(id);
        this.Log(nameof(GetCard),"Result", card);
        if(card is null)
            return new NotFoundObjectResult(new GetCardResponse { Error = "Card not found" });
        return new OkObjectResult(card.ToPresentationGet());
    }

    [HttpPost("boards/{boardId}/column/{columnId}/cards"), CustomAuthentication]
    public async Task<ActionResult<CreateCardResponse>> CreateCard([FromRoute] string boardId, string columnId, CreateCardRequest cardRequest)
    {
        this.Log(nameof(CreateCard), "Start", cardRequest);
        var createdCard = await _cardService.CreateCard(cardRequest.ToApplication());
        this.Log(nameof(CreateCard), "Result", createdCard);
        return new OkObjectResult(createdCard.ToPresentationCreate());
    }

    [HttpDelete("cards/{id}"), CustomAuthentication]
    public async Task<ActionResult> DeleteCard([FromRoute] string id)
    {
        this.Log(nameof(CreateCard), "Start", id);
        var result = await _cardService.DeleteCard(id);
        this.Log(nameof(CreateCard), "Result", new { deleted = result });
        if (result)
            return new OkObjectResult(Constants.CardDeleted);
        return new NotFoundObjectResult(Constants.CardNotFound);
    }

    [HttpPut("cards/{id}"), CustomAuthentication]
    public async Task<ActionResult> UpdateCard([FromRoute] string id, UpdateCardRequest cardRequest)
    {
        this.Log(nameof(UpdateCard), "Start", cardRequest.Card);
        if (id != cardRequest.Card.Id)
            return new BadRequestObjectResult(Constants.CardIdMissmatch);
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