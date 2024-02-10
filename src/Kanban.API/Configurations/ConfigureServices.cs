using Kanban.Application.Interfaces;
using Kanban.Application.Services;
using Kanban.Repository.Interfaces;
using Kanban.Repository.Worker;

namespace Kanban.API.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IKanbanDatabaseWorker, KanbanDatabaseWorker>();
        return services;
    }
}
