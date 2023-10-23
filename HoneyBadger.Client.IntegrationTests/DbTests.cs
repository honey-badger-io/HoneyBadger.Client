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
        await _db.CreateAsync("in-memory-db", CreateDbOptions.InMemory());
        await _db.DropAsync("in-memory-db");
    }
    
    [Fact]
    public async Task CreateOnDiskDb()
    {
        // Act
        await _db.CreateAsync("on-disk-db", CreateDbOptions.OnDisk());
        await _db.DropAsync("on-disk-db");
    }
    
    [Fact]
    public async Task ShouldNotCreateTheSameDb()
    {
        // Arrange
        const string db = "to-drop";
        var options = CreateDbOptions.InMemory();
        
        // Act
        await _db.CreateAsync(db, options);
        var ex = await Assert.ThrowsAsync<RpcException>(() => _db.CreateAsync(db, options));
        await _db.DropAsync(db);
        
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
        await _db.CreateAsync(db, options);
        await _db.EnsureAsync(db, options);
        await _db.DropAsync(db);
    }
}
