namespace Infrastructure.ServicesConfiguration;

public interface IKeyedDbContext
{
    static virtual string Key { get; }
}