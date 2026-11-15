using MemoryPack;

namespace DuskOfEmpire.Net;

[MemoryPackable]
public partial struct MatchSnapshot
{
    public int TickIndex { get; set; }

    public static byte[] Pack(int tickIndex) =>
        MemoryPackSerializer.Serialize(new MatchSnapshot { TickIndex = tickIndex });
}

public static class LanSocketCheck
{
    public static void Run()
    {
        using var host = new LanHost();
        host.Open(0);
        using var client = new LanClient();
        client.Open();
        var back = MemoryPackSerializer.Deserialize<MatchSnapshot>(MatchSnapshot.Pack(1));
        if (back.TickIndex != 1)
            throw new InvalidOperationException("snapshot roundtrip");
    }
}
