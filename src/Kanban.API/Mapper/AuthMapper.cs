using App = Kanban.Application.Dto.Models;
using Api = Kanban.API.Dto.Auth;

namespace Kanban.API.Mapper;

public static class AuthMapper
{
    public static App.ClientDto ToApplication(this Api.ClientDto client)
    {
        return new App.ClientDto
        {
            Id = client.Id,
            Secret = client.Secret,
        };
    }
}
