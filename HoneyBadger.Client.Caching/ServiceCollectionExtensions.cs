using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace HoneyBadger.Client.Caching;

public static class ServiceCollectionExtensions
{
    public static void AddHoneyBadgerDistributedCache(
        this IServiceCollection services,
        string address,
        string db,
        CreateDbOptions? options = null)
    {
        services.AddSingleton<IDistributedCache>(_ =>
            new HoneyBadgerDistributedCache(address, db, options ?? CreateDbOptions.InMemory()));
    }
}
