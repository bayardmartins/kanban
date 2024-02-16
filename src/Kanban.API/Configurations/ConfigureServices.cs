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
        services.AddScoped<ICardsDatabaseWorker, CardsDatabaseWorker>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthDatabaseWorker, AuthDatabaseWorker>();
        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IBoardsDatabaseWorker, BoardsDatabaseWorker>();
        services.AddScoped<IColumnService, ColumnService>();
        return services;
    }
}
