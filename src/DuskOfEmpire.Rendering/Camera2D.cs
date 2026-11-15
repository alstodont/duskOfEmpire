using Microsoft.Xna.Framework;
using SimVec2 = System.Numerics.Vector2;

namespace DuskOfEmpire.Rendering;

public sealed class Camera2D
{
    public const float PixelsPerUnit = 64f;

    public Vector2 Position;

    public Matrix View(int screenWidth, int screenHeight) =>
        Matrix.CreateTranslation(-Position.X, -Position.Y, 0f)
        * Matrix.CreateScale(PixelsPerUnit, -PixelsPerUnit, 1f)
        * Matrix.CreateTranslation(screenWidth * 0.5f, screenHeight * 0.5f, 0f);

    public void LookAt(SimVec2 world) => Position = new Vector2(world.X, world.Y);

    public SimVec2 ScreenToWorld(Vector2 screen, int screenWidth, int screenHeight)
    {
        var world = Vector2.Transform(screen, Matrix.Invert(View(screenWidth, screenHeight)));
        return new SimVec2(world.X, world.Y);
    }
}
