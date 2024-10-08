using Swashbuckle.AspNetCore.SwaggerUI;

namespace Bymse.Articles.BFFs.PublicApi.Configuration;

public static class PublicApiSwaggerUiConfiguration
{
    public static SwaggerUIOptions ConfigurePublicApi(this SwaggerUIOptions options)
    {
        options.SwaggerEndpoint($"/swagger/{PublicApiConstants.DocumentName}/swagger.json",
            PublicApiConstants.DocumentName);

        return options;
    }
}