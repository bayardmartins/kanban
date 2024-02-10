using App = Kanban.Application.Dto.Models;
using Repo = Kanban.Repository.Dto.Models;

namespace Kanban.Application.Dto.Mapper;

public static class ClientMapper
{
    public static Repo.ClientDto ToDatabase(this App.ClientDto client)
    {
        return new Repo.ClientDto
        {
            Id = client.Id,
            Secret = client.Secret,
        };
    }
}
