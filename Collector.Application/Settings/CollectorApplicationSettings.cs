namespace Collector.Application.Settings;

public class CollectorApplicationSettings
{
    public const string Path = CollectorConstants.Key;
    
    public string Domain { get; init; } = null!;
}