using System.Text.Json;
using System.Text.Json.Serialization;
using Bymse.Articles.BFFs.AuthN;
using Microsoft.OpenApi.Models;

namespace Bymse.Articles.BFFs.Public.Configuration;

public static class PublicApiServicesConfiguration
{
    public static IServiceCollection AddPublicApiServices(this IServiceCollection services)
    {
        services
            .AddProblemDetails()
            .AddRouting(e =>
            {
                e.LowercaseUrls = true;
                e.LowercaseQueryStrings = true;
            })
            .AddProblemDetails()
            .AddControllers(e =>
            {
                e.Filters.Add<ValidationExceptionFilter>();
            })
            .AddJsonOptions(e =>
            {
                e.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                e.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services
            .AddStubAuthN();

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(e =>
            {
                e.SwaggerDoc(PublicApiConstants.DocumentName, new OpenApiInfo
                {
                    Title = PublicApiConstants.DocumentName,
                });
            })
            ;

        return services;
    }
}