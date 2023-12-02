using System.Buffers;

namespace SealOrder.Internals;

public class P2PConnect
{
    private readonly Pipe pipe = new();

    private readonly PipeWriter writer;

    private readonly PipeReader reader;

    private readonly CancellationTokenSource source = new();

    public Socket MainSocket { get; } = new(SocketType.Stream, ProtocolType.Tcp);

    public Socket? AcceptedSocket { get; private set; }

    public Socket Socket { get => AcceptedSocket ?? MainSocket; }

    public int Port => IPEndPoint.Parse(Socket.LocalEndPoint?.ToString()!).Port;

    public bool IsClosed => source.IsCancellationRequested;

    public event Action<Exception>? ExceptionOccured;

    public P2PConnect()
    {
        writer = pipe.Writer;
        reader = pipe.Reader;
    }

    public void AsServer()
    {
        if (IsClosed) throw new InvalidOperationException();
        Socket.Bind(new IPEndPoint(IPAddress.IPv6Any, 0));
        Socket.Listen();
        Task.Run(async () =>
        {
            try
            {
                AcceptedSocket = await Socket.AcceptAsync(source.Token);
                while (!IsClosed)
                {
                    var length = await AcceptedSocket.ReceiveAsync(writer.GetMemory(), source.Token);
                    writer.Advance(length);
                    await writer.FlushAsync();
                }
            }
            catch (Exception e)
            {
                ExceptionOccured?.Invoke(e);
            }
            finally
            {
                AcceptedSocket?.Close();
                AcceptedSocket?.Dispose();
            }
        });
    }

    public async Task AsClient(string host, int port)
    {
        if (IsClosed) throw new InvalidOperationException();
        await Socket.ConnectAsync(host, port, source.Token);
        _ = Task.Run(async () =>
        {
            try
            {
                while (!IsClosed)
                {
                    var length = await Socket.ReceiveAsync(writer.GetMemory(), source.Token);
                    writer.Advance(length);
                    await writer.FlushAsync();
                }
            }
            catch (Exception e)
            {
                ExceptionOccured?.Invoke(e);
            }
        });
    }
    
    public async void Received(Action<ReadOnlySequence<byte>> action)
    {
        try
        {
            while (!IsClosed) action.Invoke(await ReceiveAsync());
        }
        catch (Exception e)
        {
            ExceptionOccured?.Invoke(e);
        }
    }

    public async Task<ReadOnlySequence<byte>> ReceiveAsync()
    {
        var result = await reader.ReadAsync(source.Token);
        var buffer = result.Buffer;
        reader.AdvanceTo(buffer.End);
        return buffer;
    }

    public void Close()
    {
        if (IsClosed) return;
        writer.CancelPendingFlush();
        reader.CancelPendingRead();
        source.Cancel();
        MainSocket.Close();
        MainSocket.Dispose();
    }
}