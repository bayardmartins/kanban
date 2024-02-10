using Kanban.Repository.Dto.Models;

namespace Kanban.Repository.Interfaces
{
    public interface IAuthDatabaseWorker
    {
        public Task<ClientDto?> GetClientById(string id);

        public Task RegisterClient(ClientDto client);
    }
}
