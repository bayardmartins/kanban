using System.Security.Principal;

namespace Kanban.API.Authentication;

public class AuthenticationClient : IIdentity
{
    public string? AuthenticationType { get; set; }

    public bool IsAuthenticated { get; set; }

    public string? Name { get; set; }
}
