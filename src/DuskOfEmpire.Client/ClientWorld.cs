using DuskOfEmpire.Core.Config;
using DuskOfEmpire.Core.Map;
using DuskOfEmpire.Sim;

namespace DuskOfEmpire.Client;

public sealed class ClientWorld
{
    public ClientWorld(IMatchConfig match, MapData map) => Host = new HostWorld(match, map);

    public HostWorld Host { get; }
}
