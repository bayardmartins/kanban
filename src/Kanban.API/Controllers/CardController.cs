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
public class CardController : ControllerBase
{
    private readonly ILogger<CardController> _logger;
    private readonly ICardService _cardService;

    public CardController(ILogger<CardController> logger, ICardService cardService)
    {
        _logger = logger;
        _cardService = cardService;
    }

    [HttpGet, CustomAuthentication]
    public async Task<ActionResult<CardResponseDto>> GetAllCards()
    {
        this._logger.LogInformation($"{nameof(CardController)}.{nameof(GetAllCards)}: Start");
        var cards = await _cardService.GetCards();
        this._logger.LogInformation($"{nameof(CardController)}.{nameof(GetAllCards)}: Result", new { cards });
        return cards.ToPresentationResponse();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CardResponseDto>> GetCard([FromRoute] string id)
    {
        this._logger.LogInformation($"{nameof(CardController)}.{nameof(GetCard)}: Start", new { id });
        var card = await _cardService.GetCardById(id);
        this._logger.LogInformation($"{nameof(CardController)}.{nameof(GetCard)}: Result", new { card });
        return card.ToPresentationResponse();
    }
}