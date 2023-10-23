using System.Runtime.InteropServices.ComTypes;
using Grpc.Core;
using HoneyBadger.Client.Hb;

namespace HoneyBadger.Client.Internal;

internal class HoneyBadgerDb : IHoneyBadgerDb
{
    private readonly Db.DbClient _dbClient;
    
    internal HoneyBadgerDb(ChannelBase channel)
    {
        _dbClient = new Db.DbClient(channel);
    }

    public async Task CreateAsync(string name, CreateDbOptions opt)
    {
        Guard.NotNullOrEmpty(nameof(name), name);

        await _dbClient.CreateAsync(new CreateDbReq
        {
            Name = name,
            Opt = new CreateDbOpt
            {
                InMemory = opt.InMem,
            },
        });
    }

    public async Task DropAsync(string name)
    {
        await _dbClient.DropAsync(new DropDbRequest
        {
            Name = name,
        });
    }

    public async Task EnsureAsync(string name, CreateDbOptions opt)
    {
        await _dbClient.EnsureDbAsync(new CreateDbReq
        {
            Name = name,
            Opt = new CreateDbOpt
            {
                InMemory = opt.InMem,
            }
        });
    }

    public void Ensure(string name, CreateDbOptions opt)
    {
        _dbClient.EnsureDb(new CreateDbReq
        {
            Name = name,
            Opt = new CreateDbOpt
            {
                InMemory = opt.InMem,
            }
        });
    }
}
