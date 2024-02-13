using Kanban.Model.Dto.API.Auth;
using Kanban.Model.Mapper.Auth;
using Kanban.CrossCutting;

namespace Kanban.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterClientRequest clientrequest)
    {
        await _authService.RegisterClient(clientrequest.Client.ToApplication());
        return new OkObjectResult(Constants.ClientRegitered);
    }
}
