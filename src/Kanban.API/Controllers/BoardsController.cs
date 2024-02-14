using Amazon.Runtime.Internal;
using Kanban.Model.Dto.API.Board;
using Kanban.Model.Mapper.Board;

namespace Kanban.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
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
    public async Task<ActionResult<GetBoardResponse>> GetBoard(string id)
    {
        this.Log(nameof(GetBoard), "Start", id);
        var board = await _boardService.GetBoard(id);
        this.Log(nameof(GetBoard), "Result", board);
        if (board is null)
            return new NotFoundResult();
        return new OkObjectResult(board.ToPresentationGet());
    }

    [HttpPost, CustomAuthentication]
    public async Task<ActionResult<CreateBoardResponse>> CreateBoard(CreateBoardRequest request)
    {
        this.Log(nameof(CreateBoard), "Start", request);
        var board = await _boardService.CreateBoard(request.ToApplication());
        this.Log(nameof(CreateBoard), "Result", board);
        return new OkObjectResult(board.ToPresentationCreate());
    }

    [HttpPut, CustomAuthentication]
    public async Task<ActionResult<UpdateBoardResponse>> UpdateBoard(UpdateBoardRequest request)
    {
        this.Log(nameof(UpdateBoard), "Start", request);
        var board = await _boardService.UpdateBoard(request.ToApplicationUpdate());
        this.Log(nameof(CreateBoard), "Result", board);
        if (board is not null)
            return new OkObjectResult(new UpdateBoardResponse { Board = board.ToPresentation() });
        return new NotFoundResult();
    }

    [HttpDelete("{id}"), CustomAuthentication]
    public async Task<ActionResult> DeleteBoard([FromRoute] string id)
    {
        this.Log(nameof(DeleteBoard), "Start", id);
        bool result = await _boardService.DeleteBoard(id);
        this.Log(nameof(DeleteBoard), "Result", result);
        if (result)
            return new OkObjectResult("message");
        return new NotFoundObjectResult("message");
    }

    private void Log(string methodName, string stage, object? data)
    {
        this._logger.LogInformation($"{methodName}: {stage}", () => new { data });
    }
}
