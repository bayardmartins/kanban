using Kanban.Repository.Models;
using Kanban.Repository.Repositories;
using Kanban.Repository.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Kanban.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    private readonly ILogger<CardController> _logger;
    private readonly IMongoRepository _cardRepository;
    private readonly IMongoSettings _mongoSettings;

    public CardController(ILogger<CardController> logger, IMongoRepository repository, IMongoSettings settings)
    {
        _logger = logger;
        _cardRepository = repository;
        _mongoSettings = settings;
    }

    [HttpGet]
    public async Task<string> Get()
    {
        var card = new Card { test = "testando" };
        var test = await _cardRepository.Insert(_mongoSettings.KanbanHost.ClusterId, _mongoSettings.KanbanHost.Database, _mongoSettings.Collections.Cards, card.ToBsonDocument());
        return "Hello World";
    }
}