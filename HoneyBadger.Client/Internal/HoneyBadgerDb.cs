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

    public async Task Create(string name, CreateDbOptions opt)
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

    public async Task Drop(string name)
    {
        await _dbClient.DropAsync(new DropDbRequest
        {
            Name = name,
        });
    }

    public async Task Ensure(string name, CreateDbOptions opt)
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
}
