using System.Numerics;
using Arch.Core;
using DuskOfEmpire.Core.Map;

namespace DuskOfEmpire.Core.Sim;

public interface ISimClock
{
    float TickRate { get; }
    int TickIndex { get; }
    void Step(float fixedDt);
}

public interface ISimSystem
{
    void Update(in SimContext ctx);
}

public interface IRenderSystem
{
    void Render(in RenderContext ctx);
}

public readonly struct InputFrame
{
    public InputFrame(Vector2 move, Vector2 aimWorld)
    {
        Move = move;
        AimWorld = aimWorld;
    }

    public Vector2 Move { get; }
    public Vector2 AimWorld { get; }
}

public interface IInputSource
{
    InputFrame Current { get; }
}

public interface ICollisionWorld
{
    Vector2 Move(Vector2 position, Vector2 halfExtents, Vector2 delta);
}

public readonly struct SimContext
{
    public float FixedDt { get; init; }
    public InputFrame Input { get; init; }
    public World World { get; init; }
    public Entity Player { get; init; }
    public ICollisionWorld Collision { get; init; }
}

public readonly struct RenderContext
{
    public Vector2 PlayerPosition { get; init; }
    public float PlayerFacingRadians { get; init; }
    public IReadOnlyList<ICollider> Colliders { get; init; }
}
