using Kanban.API.Dto.Card;
using Kanban.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Kanban.API.Mapper;
using Microsoft.AspNetCore.Authorization;
using Kanban.API.Authentication;

namespace Kanban.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
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
    public async Task<ActionResult<GetCardResponseDto>> GetAllCards()
    {
        this.Log(nameof(GetAllCards),"Start", null);
        var cards = await _cardService.GetCards();
        this.Log(nameof(GetAllCards),"Result", cards);
        return cards.ToPresentationGetResponse();
    }

    [HttpGet("{id}"), CustomAuthentication]
    public async Task<ActionResult<GetCardResponseDto>> GetCard([FromRoute] string id)
    {
        this.Log(nameof(GetCard), "Start",id);
        var card = await _cardService.GetCardById(id);
        this.Log(nameof(GetCard),"Result", card);
        return card.ToPresentationGetResponse();
    }

    [HttpPost, CustomAuthentication]
    public async Task<ActionResult<CreateCardResponseDto>> CreateCard(CardDto card)
    {
        this.Log(nameof(CreateCard), "Start", card);
        var createdCard = await _cardService.CreateCard(card.ToApplication());
        this.Log(nameof(CreateCard), "Result", createdCard);
        return createdCard.ToPresentationCreateResponse();
    }

    [HttpDelete("{id}"), CustomAuthentication]
    public async Task<ActionResult> DeleteCard([FromRoute] string id)
    {
        this.Log(nameof(CreateCard), "Start", id);
        var result = await _cardService.DeleteCard(id);
        this.Log(nameof(CreateCard), "Result", new { deleted = result });
        if (result)
            return new OkResult();
        return new NotFoundResult();
    }

    [HttpPut("{id}"), CustomAuthentication]
    public async Task<ActionResult> UpdateCard([FromRoute] string id, CardDto card)
    {
        this.Log(nameof(UpdateCard), "Start", card);
        var result = await _cardService.UpdateCard(card.ToApplication(id));
        this.Log(nameof(UpdateCard), "Result", result);
        if (result is not null)
            return new OkResult();
        return new NotFoundResult();
    }

    private void Log(string methodName, string stage, object? data)
    {
        this._logger.LogInformation($"{methodName}: {stage}", () => new { data });
    }
}