using Bymse.Articles.Apis.Public.Configuration;
using JetBrains.Annotations;
using Microsoft.AspNetCore;

namespace Bymse.Articles.Apis;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class SwaggerWebHostFactory
{
    public static IWebHost CreateWebHost()
    {
        return WebHost.CreateDefaultBuilder([])
            .ConfigureServices(services =>
            {
                services
                    .AddPublicApiServices();
            })
            .Configure(app =>
            {
                app
                    .UseExceptionHandler()
                    .UseAuthentication()
                    .UseAuthorization();

                app
                    .UseSwagger()
                    .UseSwaggerUI(e => { e.ConfigurePublicApi(); });

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            })
            .Build();
    }
}
