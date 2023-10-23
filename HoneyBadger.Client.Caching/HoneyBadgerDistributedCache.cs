using Microsoft.Extensions.Caching.Distributed;

namespace HoneyBadger.Client.Caching;

internal class HoneyBadgerDistributedCache : IDistributedCache
{
    private readonly IHoneyBadgerClient _client;
    private readonly string _db;
    private readonly CreateDbOptions _options;
    private bool _isDbOk;
    
    internal HoneyBadgerDistributedCache(string address, string db, CreateDbOptions options)
    {
        _db = db;
        _options = options;
        _client = new HoneyBadgerClient(address);
    }

    public byte[]? Get(string key)
    {
        EnsureDb();
        return _client.Data.Get(_db, key);
    }

    public async Task<byte[]?> GetAsync(string key, CancellationToken token = new CancellationToken())
    {
        await EnsureDbAsync();
        return await _client.Data.GetAsync(_db, key, token);
    }


    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        EnsureDb();
        _client.Data.Set(_db, key, value, ToTtlTimeSpan(options));
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
        CancellationToken token = new CancellationToken())
    {
        await EnsureDbAsync();
        await _client.Data.SetAsync(_db, key, value, ToTtlTimeSpan(options), token);
    }
        

    public void Refresh(string key) =>
        throw new NotSupportedException("'Refresh' is not supported with HoneyBadger.Client");

    public Task RefreshAsync(string key, CancellationToken token = new CancellationToken()) =>
        throw new NotSupportedException("'Refresh' is not supported with HoneyBadger.Client");

    public void Remove(string key)
    {
        EnsureDb();
        _client.Data.Delete(_db, key);
    }

    public async Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
    {
        await EnsureDbAsync();
        await _client.Data.DeleteAsync(_db, key, token);
    }

    private static TimeSpan? ToTtlTimeSpan(DistributedCacheEntryOptions options) =>
        options.AbsoluteExpiration.HasValue
            ? options.AbsoluteExpiration.Value - DateTimeOffset.Now
            : options.SlidingExpiration;

    private void EnsureDb()
    {
        if (_isDbOk) return;
        
        _client.Db.Ensure(_db, _options);
        _isDbOk = true;
    }
    
    private async Task EnsureDbAsync()
    {
        if (_isDbOk) return;
        
        await _client.Db.EnsureAsync(_db, _options);
        _isDbOk = true;
    }
}
