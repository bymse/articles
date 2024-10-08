using Collector.Application.Handlers.ReceiveEmails;

namespace Bymse.Articles.Workers.Collector;

public class ReceiveEmailsBackgroundWorker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ReceiveEmailsHandler>();

        await handler.Handle(stoppingToken);
    }
}