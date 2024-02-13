using Api = Kanban.Model.Dto.API.Auth;
using App = Kanban.Model.Dto.Application.Client;
using Repo = Kanban.Model.Dto.Repository.Client;

namespace Kanban.Model.Mapper.Auth;

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
    public static Repo.ClientDto ToDatabase(this App.ClientDto client)
    {
        return new Repo.ClientDto
        {
            _id = client.Id,
            Secret = client.Secret,
        };
    }
}
