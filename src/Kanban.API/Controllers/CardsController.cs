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
        this._logger.LogInformation($"{nameof(CardsController)}.{nameof(GetAllCards)}: Start");
        var cards = await _cardService.GetCards();
        this._logger.LogInformation($"{nameof(CardsController)}.{nameof(GetAllCards)}: Result", new { cards });
        return cards.ToPresentationGetResponse();
    }

    [HttpGet("{id}"), CustomAuthentication]
    public async Task<ActionResult<GetCardResponseDto>> GetCard([FromRoute] string id)
    {
        this._logger.LogInformation($"{nameof(CardsController)}.{nameof(GetCard)}: Start", new { id });
        var card = await _cardService.GetCardById(id);
        this._logger.LogInformation($"{nameof(CardsController)}.{nameof(GetCard)}: Result", new { card });
        return card.ToPresentationGetResponse();
    }

    [HttpPost, CustomAuthentication]
    public async Task<ActionResult<CreateCardResponseDto>> CreateCard(CardDto card)
    {
        this._logger.LogInformation($"{nameof(CardsController)}.{nameof(CreateCard)}: Start", new { card });
        var createdCard = await _cardService.CreateCard(card.ToApplication());
        this._logger.LogInformation($"{nameof(CardsController)}.{nameof(GetCard)}: Result", new { card });
        return createdCard.ToPresentationCreateResponse();
    }
}