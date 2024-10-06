namespace Collector.Application.Settings;

public class CollectorApplicationSettings
{
    public const string Path = $"{CollectorConstants.Key}:application";
    
    public string Domain { get; init; } = null!;
}