using Bymse.Articles.Workers.Collector;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ReceiveEmailsBackgroundWorker>();

var host = builder.Build();
host.Run();