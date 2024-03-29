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

    [HttpGet("boards/{boardId}/columns/{columnId}/cards"), CustomAuthentication]
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
            return new NotFoundObjectResult(new GetCardResponse { Error = Constants.CardNotFound });
        return new OkObjectResult(card.ToPresentationGet());
    }

    [HttpPost("boards/{boardId}/columns/{columnId}/cards"), CustomAuthentication]
    public async Task<ActionResult<CreateCardResponse>> CreateCard([FromRoute] string boardId, string columnId, CreateCardRequest cardRequest)
    {
        this.Log(nameof(CreateCard), "Start", cardRequest);
        var createdCard = await _cardService.CreateCard(cardRequest.ToApplication());
        this.Log(nameof(CreateCard), "Result", createdCard);
        return new OkObjectResult(createdCard.ToPresentationCreate());
    }

    [HttpDelete("boards/{boardId}/columns/{columnId}/cards/{id}"), CustomAuthentication]
    public async Task<ActionResult> DeleteCard([FromRoute] string boardId, string columnId, string id)
    {
        this.Log(nameof(DeleteCard), "Start", id);
        var result = await _cardService.DeleteCard(boardId, columnId, id);
        this.Log(nameof(DeleteCard), "Result", new { deleted = result });
        if (string.IsNullOrEmpty(result.Error))
            return new OkObjectResult(Constants.CardDeleted);
        return new NotFoundObjectResult(result.Error);
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

    [HttpPut("boards/{boardId}/columns/{columnId}/cards/{cardId}/priority/{index}"), CustomAuthentication]
    public async Task<ActionResult> MoveCardInPriority([FromRoute] string boardId, string columnId, string cardId, int index)
    {
        this.Log(nameof(MoveCardInPriority), "Stard", new { boardId, columnId, cardId, index });
        var result = await _cardService.MoveCardInPriority(boardId, columnId, cardId, index);
        this.Log(nameof(MoveCardInPriority), "Result", new { boardId, columnId, cardId, index, result });
        if (result.Error is null)
            return new OkResult();
        return new BadRequestObjectResult(result.Error);
    }

    [HttpPut("boards/{boardId}/columns/{originColumnId}/cards/{cardId}/move/{destinyColumnId}"), CustomAuthentication]
    public async Task<ActionResult> MoveCardInColumn([FromRoute] string boardId, string originColumnId, string cardId, string destinyColumnId)
    {
        this.Log(nameof(MoveCardInColumn), "Stard", new { boardId, originColumnId, cardId, destinyColumnId });
        var result = await _cardService.MoveCardInColumn(boardId, originColumnId, cardId, destinyColumnId);
        this.Log(nameof(MoveCardInColumn), "Result", new { boardId, originColumnId, cardId, destinyColumnId, result });
        if (result.Error is null)
            return new OkResult();
        return new BadRequestObjectResult(result.Error);
    }


    private void Log(string methodName, string stage, object? data)
    {
        this._logger.LogInformation($"{methodName}: {stage}", () => new { data });
    }
}