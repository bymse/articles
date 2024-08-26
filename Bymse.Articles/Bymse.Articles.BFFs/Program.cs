using Bymse.Articles.BFFs.PublicApi;
using Feeder.Infrastructure;
using Identity.Infrastructure;
using Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder
    .Services
    .AddPublicApiServices();

builder
    .Services
    .AddIdentityServices()
    .AddFeederServices();

builder
    .Services
    .AddMassTransitInfrastructure();

var app = builder.Build();

app
    .UseExceptionHandler()
    .UseAuthentication()
    .UseAuthorization();

app
    .MapDefaultEndpoints()
    .MapControllers();

app.Run();