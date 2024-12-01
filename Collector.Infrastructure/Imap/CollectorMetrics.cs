using System.Diagnostics.Metrics;

namespace Collector.Infrastructure.Imap;

public class CollectorMetrics(IMeterFactory meterFactory)
{
    private readonly Counter<int> emailsCounter = meterFactory.Create("Collector").CreateCounter<int>("imap.emails_fetched");
    
    public void ReportFetched()
    {
        emailsCounter.Add(1);
    }
}