namespace Collector.Application.Entities;

public class Mailbox
{
    public Ulid Id { get; init; } = Ulid.NewUlid();
    
    public uint? UidValidity { get; private set; }
    
    public uint? LastUid { get; private set; }
    
    public void SetUid(uint lastUid, uint uidValidity)
    {
        UidValidity = uidValidity;
        LastUid = lastUid;
    }
}