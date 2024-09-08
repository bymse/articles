using Bymse.Articles.BFFs.PublicApi;
using Bymse.Articles.BFFs.PublicApi.Configuration;
using Collector.Infrastructure;
using Feeder.Infrastructure;
using Identity.Infrastructure;
using Infrastructure.DI;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder
    .Services
    .AddPublicApiServices();

builder
    .Services
    .AddIdentityServices()
    .AddFeederServices()
    .AddCollectorServices();

builder
    .Services
    .AddMassTransitInfrastructure();

var app = builder.Build();

app
    .UseExceptionHandler()
    .UseAuthentication()
    .UseAuthorization();

app
    .UseSwagger()
    .UseSwaggerUI(e =>
    {
        e.ConfigurePublicApi();
    });

app
    .MapDefaultEndpoints()
    .MapControllers();

app.Run();