using Kanban.Model.Dto.API.Column;
using Kanban.Model.Mapper.Board;

namespace Kanban.API.Controllers;

public partial class BoardsController
{
    [HttpPost("{boardId}/columns"), CustomAuthentication]
    public async Task<ActionResult> AddColumn([FromRoute] string boardId, AddColumnRequest request)
    {
        this.Log(nameof(AddColumn), "Start", new { boardId, request });
        var result = await _boardService.AddColumn(request.ToApplicationAdd(boardId));
        this.Log(nameof(AddColumn), "Result", new { result });
        if (!string.IsNullOrEmpty(result.Error))
            return new BadRequestObjectResult(result.Error);
        return new OkResult();
    }
}
