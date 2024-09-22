namespace Collector.Application.Settings;

public class CollectorApplicationSettings
{
    public const string Path = CollectorConstants.Key;
    
    public string Domain { get; init; } = null!;
    
    public string CollectorImapPassword { get; init; } = null!;
    
    public string CollectorImapUsername { get; init; } = null!;
}