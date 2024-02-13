using Kanban.CrossCutting;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace Kanban.API.Configurations;

[ExcludeFromCodeCoverage]
public static class ConfigureSwagger
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt => 
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Kaban", Version = "v1" });
            opt.AddSecurityDefinition(Constants.Authentication, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Basic Authorization Header",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = Constants.Authentication
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=Constants.Authentication,
                        },
                    },
                    new string[]{ $"{Constants.Authentication} " }
                }
            });
        });
        return services;
    }
}
