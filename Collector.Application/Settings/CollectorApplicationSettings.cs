namespace Collector.Application.Settings;

public class CollectorApplicationSettings
{
    public const string Path = $"{CollectorConstants.Key}:application";
    
    public string RootReceiver { get; init; } = null!;
}