using Kanban.API.Dto.Card;
using Kanban.Application.Interfaces;
using Kanban.Application.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Kanban.API.Mapper;

namespace Kanban.API.Controllers;

[ApiController]
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

    [HttpGet]
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