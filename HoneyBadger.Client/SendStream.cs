using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HoneyBadger.Client.Hb;

namespace HoneyBadger.Client;

public class SendStream : IDisposable
{
    private readonly AsyncClientStreamingCall<SendStreamReq, Empty> _grpcStream;

    internal SendStream(AsyncClientStreamingCall<SendStreamReq, Empty> grpcStream)
    {
        _grpcStream = grpcStream;
    }

    public Task Write(string key, byte[] data, CancellationToken ct = default) =>
        _grpcStream.RequestStream.WriteAsync(new SendStreamReq
        {
            Item = new DataItem
            {
                Key = key,
                Data = ByteString.CopyFrom(data),
            }
        }, cancellationToken: ct);
    
    public Task Write(string key, string data, CancellationToken ct = default) =>
        _grpcStream.RequestStream.WriteAsync(new SendStreamReq
        {
            Item = new DataItem
            {
                Key = key,
                Data = ByteString.CopyFromUtf8(data),
            }
        }, cancellationToken: ct);

    public async Task Close()
    {
        await _grpcStream.RequestStream.CompleteAsync();
        await _grpcStream.ResponseAsync;        
    }

    public void Dispose()
    {
        _grpcStream.Dispose();
    }
}
