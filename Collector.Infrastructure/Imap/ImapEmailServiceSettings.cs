using Collector.Application;

namespace Collector.Infrastructure.Imap;

public class ImapEmailServiceSettings
{
    public const string Path = $"{CollectorConstants.Key}:imap";

    public string Hostname { get; init; } = null!;
    public int Port { get; init; }
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}