using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Layers;

namespace DuskOfEmpire.Net;

public sealed class LanHost : IDisposable
{
    private readonly LiteNetManager _net = new(new QuietListener(), null);

    public LanHost() => _net.IPv6Enabled = false;

    public bool IsOpen => _net.IsRunning;

    public void Open(int port)
    {
        if (!_net.Start(port))
            throw new InvalidOperationException("host socket did not open");
    }

    public void Close()
    {
        if (_net.IsRunning)
            _net.Stop();
    }

    public void Dispose() => Close();
}

public sealed class LanClient : IDisposable
{
    private readonly LiteNetManager _net = new(new QuietListener(), null);

    public LanClient() => _net.IPv6Enabled = false;

    public bool IsOpen => _net.IsRunning;

    public void Open()
    {
        if (!_net.Start())
            throw new InvalidOperationException("client socket did not open");
    }

    public void Connect(string host, int port) => _net.Connect(host, port, "duskOfEmpire");

    public void Close()
    {
        if (_net.IsRunning)
            _net.Stop();
    }

    public void Dispose() => Close();
}

sealed class QuietListener : ILiteNetEventListener
{
    public void OnPeerConnected(LiteNetPeer peer) { }
    public void OnPeerDisconnected(LiteNetPeer peer, DisconnectInfo disconnectInfo) { }
    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) { }
    public void OnNetworkReceive(LiteNetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) { }
    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) { }
    public void OnNetworkLatencyUpdate(LiteNetPeer peer, int latency) { }
    public void OnConnectionRequest(LiteConnectionRequest request) { }
    public void OnPeerAddressChanged(LiteNetPeer peer, IPEndPoint previousAddress) { }
    public void OnMessageDelivered(LiteNetPeer peer, object userData) { }
}
