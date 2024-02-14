using Kanban.Model.Dto.API.Column;
using Kanban.Model.Mapper.Board;

namespace Kanban.API.Controllers;

[ApiController]
[Authorize]
[Route("boards/{boardId}/columns")]
public class ColumnsController : ControllerBase
{
    private readonly ILogger<ColumnsController> _logger;
    private readonly IColumnService _columnService;

    public ColumnsController(ILogger<ColumnsController> logger, IColumnService columnService)
    {
        _columnService = columnService;
        _logger = logger;
    }

    [HttpPost, CustomAuthentication]
    public async Task<ActionResult> AddColumn([FromRoute] string boardId, AddColumnRequest request)
    {
        this.Log(nameof(AddColumn), "Start", new { boardId, request });
        var result = await _columnService.AddColumn(request.ToApplicationAdd(boardId));
        this.Log(nameof(AddColumn), "Result", new { result });
        if (!string.IsNullOrEmpty(result.Error))
            return new BadRequestObjectResult(result.Error);
        return new OkResult();
    }

    private void Log(string methodName, string stage, object? data)
    {
        this._logger.LogInformation($"{methodName}: {stage}", () => new { data });
    }
}
