using Kanban.Model.Dto.API.Column;
using Kanban.Model.Mapper.Board;
using Kanban.Model.Mapper.Column;

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
        var result = await _columnService.AddColumn(request.ToApplication(boardId));
        this.Log(nameof(AddColumn), "Result", new { result });
        if (!string.IsNullOrEmpty(result.Error))
            return new BadRequestObjectResult(new ColumnActionResponse { Error = result.Error });
        return new OkObjectResult(new ColumnActionResponse { ColumnId = result.ColumnId });
    }

    [HttpPut("{id}"), CustomAuthentication]
    public async Task<ActionResult> UpdateColumn([FromRoute] string boardId, string id, UpdateColumnRequest request)
    {
        this.Log(nameof(UpdateColumn), "Start", null);
        var result = await _columnService.UpdateColumn(request.ToApplication(boardId, id));
        this.Log(nameof(UpdateColumn), "Result", new { result });
        if (!string.IsNullOrEmpty(result.Error))
            return new BadRequestObjectResult(result.Error);
        return new OkResult();
    }

    [HttpDelete("{id}"), CustomAuthentication]
    public async Task<ActionResult> DeleteColumns(string boardId, string id)
    {
        this.Log(nameof(DeleteColumns), "Start", new { boardId, id });
        var result = await _columnService.DeleteColumn(boardId, id);
        this.Log(nameof(DeleteColumns), "Result", result );
        if (result.Error is not null)
            return new BadRequestObjectResult(result.Error);
        return new OkObjectResult(result.ColumnId);
    }

    private void Log(string methodName, string stage, object? data)
    {
        this._logger.LogInformation($"{methodName}: {stage}", () => new { data });
    }
}
