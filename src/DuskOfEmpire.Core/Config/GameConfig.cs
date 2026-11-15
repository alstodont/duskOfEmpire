namespace DuskOfEmpire.Core.Config;

public sealed class MemorySection;

public sealed class PvpveSection;

public sealed class WombSection;

public sealed class HerderSection;

public sealed class ItemSection;

public sealed class SlotSection;

public sealed class LootSection
{
    public float GroundCleanupSec { get; init; } = 300f;
    public float TopLootWindowSec { get; init; } = 600f;
    public float ShooterConsumableCooldownSec { get; init; } = 30f;
}

public sealed class GameConfig
{
    public MemorySection Memory { get; init; } = new();
    public PvpveSection Pvpve { get; init; } = new();
    public WombSection Womb { get; init; } = new();
    public HerderSection Herder { get; init; } = new();
    public ItemSection Item { get; init; } = new();
    public SlotSection Slots { get; init; } = new();
    public LootSection Loot { get; init; } = new();
    public int FriendlyFirePct { get; init; } = 10;
    public int RespawnPvpveSec { get; init; } = 30;
}
