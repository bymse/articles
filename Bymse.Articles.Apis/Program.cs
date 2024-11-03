using Bymse.Articles.Apis.Public.Configuration;
using Bymse.Articles.Database;
using Collector.Infrastructure;
using Feeder.Infrastructure;
using Identity.Infrastructure;
using Infrastructure.ServicesConfiguration;

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
    .AddMassTransitInfrastructure<ArticlesDbContext>();

builder.Services.AddPostgresDbContext<ArticlesDbContext>();
builder.EnrichNpgsqlDbContext<ArticlesDbContext>();

var app = builder.Build();

app
    .UseExceptionHandler()
    .UseAuthentication()
    .UseAuthorization();

app
    .UseSwagger()
    .UseSwaggerUI(e => { e.ConfigurePublicApi(); });

app
    .MapDefaultEndpoints()
    .MapControllers();

Thread.Sleep(TimeSpan.FromSeconds(20));
app.Run();