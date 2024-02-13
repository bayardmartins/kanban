using Kanban.Model.Dto.Application.Client;

namespace Kanban.Application.Interfaces;

public interface IAuthService
{
    public Task RegisterClient(ClientDto client);

    public Task<bool> Login(ClientDto client);
}
