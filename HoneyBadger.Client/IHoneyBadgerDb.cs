namespace HoneyBadger.Client;

public interface IHoneyBadgerDb
{
    Task CreateAsync(string name, CreateDbOptions opt);
    
    Task DropAsync(string name);

    Task EnsureAsync(string name, CreateDbOptions opt);
    
    void Ensure(string name, CreateDbOptions opt);
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
