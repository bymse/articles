using Collector.Application.Handlers.ReceiveEmails;

namespace Bymse.Articles.Workers.Collector;

public class ReceiveEmailsBackgroundWorker(
    IServiceProvider serviceProvider,
    ILogger<ReceiveEmailsBackgroundWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<ReceiveEmailsHandler>();

                await handler.Handle(stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occurred while handling emails");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}