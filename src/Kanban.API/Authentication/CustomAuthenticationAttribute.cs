using Kanban.CrossCutting;
using Microsoft.AspNetCore.Authorization;

namespace Kanban.API.Authentication;

public class CustomAuthenticationAttribute : AuthorizeAttribute
{
    public CustomAuthenticationAttribute() 
    {
        AuthenticationSchemes = Constants.Authentication;
    }
}
