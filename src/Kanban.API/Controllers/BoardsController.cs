namespace Kanban.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller")]
public class BoardsController
{
    private readonly ILogger<BoardsController> _logger;
    private readonly IBoardService _boardService;

    public BoardsController(ILogger<BoardsController> logger, IBoardService boardService) 
    { 
        _logger = logger;
        _boardService = boardService;
    }

    [HttpGet("{id}"), CustomAuthentication]
    public async Task<ActionResult> GetBoard(string id)
    {
        this.Log(nameof(GetBoard), "Start", id);
        var board = await _boardService.GetBoard(id);
        this.Log(nameof(GetBoard), "Result", board);
        if (board is null)
            return new NotFoundResult();
        return new OkObjectResult(board);
    }

    private void Log(string methodName, string stage, object? data)
    {
        this._logger.LogInformation($"{methodName}: {stage}", () => new { data });
    }
}
