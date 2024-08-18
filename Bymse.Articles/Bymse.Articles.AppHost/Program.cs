var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Bymse_Articles_BFFs>("webfrontend")
    .WithExternalHttpEndpoints();

builder.Build().Run();