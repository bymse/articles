var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Bymse_Articles_ApiService>("apiservice");

builder.AddProject<Projects.Bymse_Articles_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();