using Bymse.Articles.BFFs.PublicApi;
using Bymse.Articles.BFFs.PublicApi.Configuration;
using Bymse.Articles.Database;
using Collector.Infrastructure;
using Collector.Infrastructure.Database;
using Feeder.Infrastructure;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure;
using Identity.Infrastructure.Database;
using Infrastructure.ServicesConfiguration;
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
    .AddMassTransitInfrastructure();

builder.Services.AddPostgresDbContext<ArticlesDbContext>();
builder.EnrichNpgsqlDbContext<ArticlesDbContext>();

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