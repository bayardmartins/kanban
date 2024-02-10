using Microsoft.OpenApi.Models;

namespace Kanban.API.Configurations;

public static class ConfigureSwagger
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt => 
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Kaban", Version = "v1" });
            opt.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Basic Authorization Header",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Basic"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Basic"
                        },
                    },
                    new string[]{ "Basic "}
                }
            });
        });
        return services;
    }
}
