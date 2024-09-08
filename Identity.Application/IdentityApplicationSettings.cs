namespace Identity.Application;

public class IdentityApplicationSettings
{
    public const string Path = IdentityConstants.Key;
    
    public string StubUserId { get; init; } = null!;
}