using Application.Mediator;
using Collector.Application.UseCases.ReceiveEmails;
using MassTransit.Mediator;

namespace Bymse.Articles.Workers.Collector;

public class ReceiveEmailsBackgroundWorker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        await sender.Send(new ReceiveEmailsUseCase(), stoppingToken);
    }
}