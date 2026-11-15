using WinCAN.Core;

namespace WinCAN.Connections;

/// <summary>
/// Aggregates live connections and owns the global bus numbering used by the
/// application (the role CanConManager plays in SavvyCAN). Frames arriving from a
/// transport are re-stamped with their global bus number before being published.
/// </summary>
public sealed class CanConnectionManager : IAsyncDisposable
{
    private readonly Lock _lock = new();
    private readonly List<Entry> _entries = [];
    private int _nextBus;

    public event EventHandler<CanFrame>? FrameReceived;

    public event EventHandler? ConnectionsChanged;

    public IReadOnlyList<Entry> Connections
    {
        get
        {
            lock (_lock)
            {
                return _entries.ToArray();
            }
        }
    }

    public sealed record Entry(ICanConnection Connection, int FirstBus, int BusCount)
    {
        public int LastBus => FirstBus + BusCount - 1;
    }

    /// <summary>Attaches a transport and gives each of its buses a global bus number.</summary>
    public Entry Attach(ICanConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        Entry entry;
        lock (_lock)
        {
            if (_entries.Any(e => ReferenceEquals(e.Connection, connection)))
            {
                return _entries.First(e => ReferenceEquals(e.Connection, connection));
            }

            var busCount = Math.Max(1, connection.Buses.Count);
            entry = new Entry(connection, _nextBus, busCount);
            _nextBus += busCount;
            _entries.Add(entry);
        }

        connection.FrameReceived += (_, frame) => Publish(entry, frame);
        ConnectionsChanged?.Invoke(this, EventArgs.Empty);
        return entry;
    }

    public void Detach(ICanConnection connection)
    {
        lock (_lock)
        {
            _entries.RemoveAll(e => ReferenceEquals(e.Connection, connection));
        }

        ConnectionsChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task ConnectAsync(ICanConnection connection, CancellationToken cancellationToken = default)
    {
        Attach(connection);
        await connection.ConnectAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DisconnectAsync(ICanConnection connection, CancellationToken cancellationToken = default)
    {
        await connection.DisconnectAsync(cancellationToken).ConfigureAwait(false);
        Detach(connection);
    }

    /// <summary>Sends a frame on the global bus number stored in <see cref="CanFrame.Bus"/>.</summary>
    public async Task SendFrameAsync(CanFrame frame, CancellationToken cancellationToken = default)
    {
        Entry? entry;
        lock (_lock)
        {
            entry = _entries.FirstOrDefault(e => frame.Bus >= e.FirstBus && frame.Bus <= e.LastBus);
        }

        if (entry is null)
        {
            throw new InvalidOperationException($"No connection owns bus {frame.Bus}.");
        }

        var localBus = frame.Bus - entry.FirstBus;
        var copy = frame.Clone();
        copy.Bus = (byte)localBus;
        await entry.Connection.SendFrameAsync(localBus, copy, cancellationToken).ConfigureAwait(false);
    }

    public async Task DisconnectAllAsync()
    {
        Entry[] entries;
        lock (_lock)
        {
            entries = _entries.ToArray();
            _entries.Clear();
        }

        foreach (var entry in entries)
        {
            try
            {
                await entry.Connection.DisconnectAsync().ConfigureAwait(false);
            }
            catch
            {
                // Disconnect is best effort; the connection is being dropped anyway.
            }

            await entry.Connection.DisposeAsync().ConfigureAwait(false);
        }

        ConnectionsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Publish(Entry entry, CanFrame frame)
    {
        var global = frame.Clone();
        global.Bus = (byte)(entry.FirstBus + frame.Bus);
        FrameReceived?.Invoke(this, global);
    }

    public async ValueTask DisposeAsync() => await DisconnectAllAsync().ConfigureAwait(false);
}
