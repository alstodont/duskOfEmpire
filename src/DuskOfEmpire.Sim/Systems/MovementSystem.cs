using System.Numerics;
using Arch.Core;
using DuskOfEmpire.Core.Ecs;
using DuskOfEmpire.Core.Sim;

namespace DuskOfEmpire.Sim.Systems;

public sealed class MovementSystem : ISimSystem
{
    // shortcut: units per second and half-extent until tile size and move speed are named
    private const float Speed = 6f;
    private static readonly Vector2 Half = new(0.4f, 0.4f);

    public void Update(in SimContext ctx)
    {
        ref var pose = ref ctx.World.Get<Pose>(ctx.Player);
        var aim = ctx.Input.AimWorld - pose.Position;
        if (aim.LengthSquared() > 0.000001f)
            pose.FacingRadians = MathF.Atan2(aim.Y, aim.X);

        var move = ctx.Input.Move;
        if (move.LengthSquared() > 1f)
            move = Vector2.Normalize(move);
        pose.Position = ctx.Collision.Move(pose.Position, Half, move * Speed * ctx.FixedDt);
    }
}
