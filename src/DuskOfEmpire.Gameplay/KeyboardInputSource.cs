using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DuskOfEmpire.Core.Sim;
using SimVec2 = System.Numerics.Vector2;

namespace DuskOfEmpire.Gameplay;

public sealed class KeyboardInputSource : IInputSource
{
    public InputFrame Current { get; private set; }

    public void Sample(SimVec2 aimWorld)
    {
        var keys = Keyboard.GetState();
        var move = SimVec2.Zero;
        if (keys.IsKeyDown(Keys.W))
            move.Y += 1f;
        if (keys.IsKeyDown(Keys.S))
            move.Y -= 1f;
        if (keys.IsKeyDown(Keys.D))
            move.X += 1f;
        if (keys.IsKeyDown(Keys.A))
            move.X -= 1f;
        Current = new InputFrame(move, aimWorld);
    }
}
