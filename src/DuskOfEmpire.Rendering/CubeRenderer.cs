using Microsoft.Xna.Framework;
using SimVec2 = System.Numerics.Vector2;
using Microsoft.Xna.Framework.Graphics;
using DuskOfEmpire.Core.Math;
using DuskOfEmpire.Core.Sim;

namespace DuskOfEmpire.Rendering;

public sealed class CubeRenderer : IRenderSystem, IDisposable
{
    private static readonly SimVec2 PlayerHalf = new(0.4f, 0.4f);
    private static readonly SimVec2 NoseHalf = new(0.12f, 0.12f);

    private readonly GraphicsDevice _device;
    private readonly PrimitivesRenderer _primitives;
    private readonly Camera2D _camera;
    private readonly ViewTargets _targets;
    private Effect? _memory;
    private Effect? _light;

    public CubeRenderer(GraphicsDevice device, Camera2D camera)
    {
        _device = device;
        _camera = camera;
        _primitives = new PrimitivesRenderer(device);
        _targets = new ViewTargets();
    }

    public ViewTargets Targets => _targets;

    public void BindEffects(Effect memory, Effect light)
    {
        _memory = memory ?? throw new ArgumentNullException(nameof(memory));
        _light = light ?? throw new ArgumentNullException(nameof(light));
        if (_memory.CurrentTechnique is null || _light.CurrentTechnique is null)
            throw new InvalidOperationException("effect stub has no technique");
    }

    public void Resize(int width, int height) => _targets.Resize(_device, width, height);

    public void Render(in RenderContext ctx)
    {
        if (_targets.Scene is null || _targets.MemoryMask is null || _targets.MemoryView is null || _targets.Light is null)
            throw new InvalidOperationException("render targets are missing");
        if (_memory is null || _light is null)
            throw new InvalidOperationException("effect stubs are missing");

        var view = _camera.View(_device.Viewport.Width, _device.Viewport.Height);
        _device.SetRenderTarget(_targets.Scene);
        _device.Clear(new Color(24, 22, 28));
        _primitives.Begin(view);
        foreach (var collider in ctx.Colliders)
            _primitives.DrawAabb(collider.Bounds, new Color(70, 68, 78));
        _primitives.DrawAabb(Aabb.FromCenter(ctx.PlayerPosition, PlayerHalf), new Color(196, 122, 58));
        var forward = new SimVec2(MathF.Cos(ctx.PlayerFacingRadians), MathF.Sin(ctx.PlayerFacingRadians));
        _primitives.DrawAabb(Aabb.FromCenter(ctx.PlayerPosition + forward * 0.55f, NoseHalf), new Color(240, 230, 210));
        _primitives.End();

        ClearTarget(_targets.MemoryMask);
        ClearTarget(_targets.MemoryView);
        ClearTarget(_targets.Light);

        _device.SetRenderTarget(null);
        _primitives.DrawFullscreen(_targets.Scene);
    }

    public void Dispose()
    {
        _primitives.Dispose();
        _targets.Dispose();
    }

    private void ClearTarget(RenderTarget2D target)
    {
        _device.SetRenderTarget(target);
        _device.Clear(Color.Black);
    }
}
