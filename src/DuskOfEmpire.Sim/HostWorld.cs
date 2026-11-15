using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using DuskOfEmpire.Core.Config;
using DuskOfEmpire.Core.Ecs;
using DuskOfEmpire.Core.Map;
using DuskOfEmpire.Core.Math;
using DuskOfEmpire.Core.Sim;
using DuskOfEmpire.Physics;
using DuskOfEmpire.Sim.Systems;

namespace DuskOfEmpire.Sim;

public sealed class HostWorld : ISimClock, IDisposable
{
    // shortcut: fixed arena until test_city.json carries colliders
    private const float ArenaHalf = 12f;
    private const float Wall = 1f;

    private readonly World _world;
    private readonly Entity _player;
    private readonly SpatialCollision _collision;
    private readonly List<ICollider> _colliders = new();
    private readonly ISimSystem[] _systems;
    private bool _disposed;

    public HostWorld(IMatchConfig match, MapData map)
    {
        Match = match;
        _world = World.Create();
        _player = _world.Create(Component<Pose>.ComponentType);
        _player.Set(new Pose());
        _collision = new SpatialCollision(4f);
        foreach (var collider in map.Colliders)
            AddSolid(collider.Bounds);
        AddArena();
        _systems = [new MovementSystem()];
    }

    public IMatchConfig Match { get; }
    public float TickRate => 30f;
    public int TickIndex { get; private set; }
    public InputFrame Input { get; set; }
    public IReadOnlyList<ICollider> Colliders => _colliders;
    public Pose PlayerPose => _world.Get<Pose>(_player);

    public void Step(float fixedDt)
    {
        var ctx = new SimContext
        {
            FixedDt = fixedDt,
            Input = Input,
            World = _world,
            Player = _player,
            Collision = _collision,
        };
        foreach (var system in _systems)
            system.Update(in ctx);
        TickIndex++;
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        World.Destroy(_world);
    }

    private void AddArena()
    {
        var h = ArenaHalf;
        var t = Wall;
        AddSolid(new Aabb(new Vector2(-h - t, -h - t), new Vector2(-h, h + t)));
        AddSolid(new Aabb(new Vector2(h, -h - t), new Vector2(h + t, h + t)));
        AddSolid(new Aabb(new Vector2(-h, -h - t), new Vector2(h, -h)));
        AddSolid(new Aabb(new Vector2(-h, h), new Vector2(h, h + t)));
    }

    private void AddSolid(Aabb box)
    {
        _colliders.Add(new BoxCollider(box));
        _collision.Add(box);
    }
}
