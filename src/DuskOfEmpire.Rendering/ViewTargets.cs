using Microsoft.Xna.Framework.Graphics;

namespace DuskOfEmpire.Rendering;

public sealed class ViewTargets : IDisposable
{
    public RenderTarget2D? Scene { get; private set; }
    public RenderTarget2D? MemoryMask { get; private set; }
    public RenderTarget2D? MemoryView { get; private set; }
    public RenderTarget2D? Light { get; private set; }

    // shortcut: Color until the glossary assigns a pixel format
    public void Resize(GraphicsDevice device, int width, int height)
    {
        if (width <= 0 || height <= 0)
            return;
        if (Scene is { IsDisposed: false, Width: var w, Height: var h } && w == width && h == height)
            return;

        Dispose();
        Scene = Create(device, width, height);
        MemoryMask = Create(device, width, height);
        MemoryView = Create(device, width, height);
        Light = Create(device, width, height);
    }

    public void Dispose()
    {
        Scene?.Dispose();
        MemoryMask?.Dispose();
        MemoryView?.Dispose();
        Light?.Dispose();
        Scene = null;
        MemoryMask = null;
        MemoryView = null;
        Light = null;
    }

    private static RenderTarget2D Create(GraphicsDevice device, int width, int height) =>
        new(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
}
