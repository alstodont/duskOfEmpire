using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DuskOfEmpire.Core.Math;
using XnaVec2 = Microsoft.Xna.Framework.Vector2;

namespace DuskOfEmpire.Rendering;

public sealed class PrimitivesRenderer : IDisposable
{
    private readonly Texture2D _pixel;
    private readonly SpriteBatch _batch;

    public PrimitivesRenderer(GraphicsDevice device)
    {
        _pixel = new Texture2D(device, 1, 1);
        _pixel.SetData([Color.White]);
        _batch = new SpriteBatch(device);
    }

    public void Begin(Matrix view) =>
        _batch.Begin(samplerState: SamplerState.LinearClamp, transformMatrix: view);

    public void DrawAabb(Aabb box, Color color)
    {
        var position = new XnaVec2(box.Min.X, box.Min.Y);
        var scale = new XnaVec2(box.Max.X - box.Min.X, box.Max.Y - box.Min.Y);
        _batch.Draw(_pixel, position, null, color, 0f, XnaVec2.Zero, scale, SpriteEffects.None, 0f);
    }

    public void End() => _batch.End();

    public void DrawFullscreen(Texture2D texture)
    {
        _batch.Begin(samplerState: SamplerState.LinearClamp);
        _batch.Draw(texture, texture.GraphicsDevice.Viewport.Bounds, Color.White);
        _batch.End();
    }

    public void Dispose()
    {
        _batch.Dispose();
        _pixel.Dispose();
    }
}
