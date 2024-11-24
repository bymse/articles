using System.Diagnostics;

namespace Collector.Infrastructure;

public static class CollectorActivitySource
{
    public static readonly ActivitySource Source = new ActivitySource("Bymse.Articles.Collector");
}