using Grpc.Core;
using Shouldly;

namespace HoneyBadger.Client.IntegrationTests;

public class DbTests
{
    private readonly IHoneyBadgerDb _db;

    public DbTests()
    {
        _db = new HoneyBadgerClient("127.0.0.1:18950").Db;
    }

    [Fact]
    public async Task CreateInMemoryDb()
    {
        // Act
        await _db.Create("in-memory-db", CreateDbOptions.InMemory());
        await _db.Drop("in-memory-db");
    }
    
    [Fact]
    public async Task CreateOnDiskDb()
    {
        // Act
        await _db.Create("on-disk-db", CreateDbOptions.OnDisk());
        await _db.Drop("on-disk-db");
    }
    
    [Fact]
    public async Task ShouldNotCreateTheSameDb()
    {
        // Arrange
        const string db = "to-drop";
        var options = CreateDbOptions.InMemory();
        
        // Act
        await _db.Create(db, options);
        var ex = await Assert.ThrowsAsync<RpcException>(() => _db.Create(db, options));
        await _db.Drop(db);
        
        // Assert
        ex.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task ShouldEnsureDb()
    {
        // Arrange
        const string db = "ensure-db-test";
        var options = CreateDbOptions.InMemory();
        
        // Act
        await _db.Create(db, options);
        await _db.Ensure(db, options);
        await _db.Drop(db);
    }
}
