namespace HoneyBadger.Client;

public interface IHoneyBadgerDb
{
    Task Create(string name, CreateDbOptions opt);
    
    Task Drop(string name);

    Task Ensure(string name, CreateDbOptions opt);
}

public class CreateDbOptions
{
    private CreateDbOptions(bool inMemory)
    {
        InMem = inMemory;
    }
    
    internal bool InMem { get; }

    public static CreateDbOptions InMemory() => new CreateDbOptions(true);
    
    public static CreateDbOptions OnDisk() => new CreateDbOptions(false);
}
