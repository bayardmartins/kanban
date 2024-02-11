using Kanban.Application.Dto.Mapper;
using Kanban.Application.Dto.Models;
using Kanban.Application.Interfaces;
using Kanban.Repository.Interfaces;

namespace Kanban.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthDatabaseWorker _databaseWorker;
    public AuthService(IAuthDatabaseWorker databaseWorker) 
    { 
        _databaseWorker = databaseWorker;
    }

    public async Task RegisterClient(ClientDto client)
    {
        await _databaseWorker.RegisterClient(client.ToDatabase());
    }

    public async Task<bool> Login(ClientDto client)
    {
        var dbClient = await _databaseWorker.GetClientById(client.Id);
        return dbClient is not null && dbClient.Secret == client.Secret;
    }
}
