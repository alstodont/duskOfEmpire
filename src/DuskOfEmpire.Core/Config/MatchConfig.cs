namespace DuskOfEmpire.Core.Config;

public interface IMatchConfig
{
    GameConfig Snapshot { get; }
    int FriendlyFirePct { get; }
    int RespawnPvpveSec { get; }
    float GroundCleanupSec { get; }
    float TopLootWindowSec { get; }
    float ShooterConsumableCooldownSec { get; }
}

public sealed class MatchConfig : IMatchConfig
{
    public MatchConfig(GameConfig snapshot) => Snapshot = snapshot;

    public GameConfig Snapshot { get; }
    public int FriendlyFirePct => Snapshot.FriendlyFirePct;
    public int RespawnPvpveSec => Snapshot.RespawnPvpveSec;
    public float GroundCleanupSec => Snapshot.Loot.GroundCleanupSec;
    public float TopLootWindowSec => Snapshot.Loot.TopLootWindowSec;
    public float ShooterConsumableCooldownSec => Snapshot.Loot.ShooterConsumableCooldownSec;
}
