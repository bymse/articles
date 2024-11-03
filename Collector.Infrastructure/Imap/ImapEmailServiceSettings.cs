using Collector.Application;

namespace Collector.Infrastructure.Imap;

public class ImapEmailServiceSettings
{
    public const string Path = $"{CollectorConstants.Key}:imap";

    public int Port { get; init; }
    public string Hostname { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public bool UseSsl { get; init; }
}