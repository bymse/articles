using System.Text.Json;
using System.Text.Json.Serialization;
using Bymse.Articles.BFFs.AuthN;

namespace Bymse.Articles.BFFs.PublicApi;

public static class PublicApiDiHelper
{
    public static IServiceCollection AddPublicApiServices(this IServiceCollection services)
    {
        services
            .AddProblemDetails()
            .AddControllers()
            .AddJsonOptions(e =>
            {
                e.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                e.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services
            .AddStubAuthN();

        return services;
    }
}