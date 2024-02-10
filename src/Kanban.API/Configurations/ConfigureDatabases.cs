using Kanban.CrossCutting;
using Kanban.Repository.Interfaces;
using Kanban.Repository.Repositories;
using Kanban.Repository.Settings;
using MongoDB.Driver;

namespace Kanban.API.Configurations;

public static class ConfigureDatabases
{
    public static IServiceCollection AddRepository(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMongoRepository, MongoRepository>();
        builder.Services.AddSingleton<IMongoSettings>(builder.Configuration.GetSection(Constants.MongoSettings).Get<MongoSettings>());
        string kanbanCS = builder.Configuration.GetSection(Constants.HostSetting).Value;
        MongoClientSettings kanbanSettings = MongoClientSettings.FromConnectionString(kanbanCS);
        kanbanSettings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
        builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(kanbanSettings));
        return builder.Services;
    }
}
