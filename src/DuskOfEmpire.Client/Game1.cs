using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DuskOfEmpire.Core.Config;
using DuskOfEmpire.Core.Map;
using DuskOfEmpire.Core.Sim;
using DuskOfEmpire.Debug;
using DuskOfEmpire.Gameplay;
using DuskOfEmpire.Net;
using DuskOfEmpire.Rendering;

namespace DuskOfEmpire.Client;

public sealed class Game1 : Game
{
    // shortcut: drop leftover time above 5 steps per frame until hitch budget is measured
    private const int MaxStepsPerFrame = 5;

    private static readonly string Version =
        typeof(Game1).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? "0.1.0";

    private readonly GraphicsDeviceManager _graphics;
    private readonly string _dataRoot;
    private readonly KeyboardInputSource _input = new();
    private readonly Camera2D _camera = new();

    private ClientWorld? _client;
    private IClientPrefs? _prefs;
    private CubeRenderer? _renderer;
    private DebugOverlay? _overlay;
    private float _accumulator;
    private bool _reloadPrefs;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        Content.RootDirectory = "Effects";
        IsMouseVisible = true;
        IsFixedTimeStep = false;
        Window.Title = "duskOfEmpire";
        _dataRoot = Path.Combine(AppContext.BaseDirectory, "data");
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        var match = new MatchConfig(ConfigLoader.LoadMatch(Path.Combine(_dataRoot, "config.json")));
        _prefs = ConfigLoader.LoadClient(Path.Combine(_dataRoot, "client.json"));
        var map = new JsonMapLoader().Load(Path.Combine(_dataRoot, "maps", "test_city.json"));
        _client = new ClientWorld(match, map);
        _renderer = new CubeRenderer(GraphicsDevice, _camera);
        _renderer.Resize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        _renderer.BindEffects(Content.Load<Effect>("memory"), Content.Load<Effect>("light"));
        _overlay = new DebugOverlay(this);
        LanSocketCheck.Run();
        Window.ClientSizeChanged += (_, _) =>
            _renderer?.Resize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (_client is null || _prefs is null || _renderer is null)
            return;

        if (_reloadPrefs)
        {
            _reloadPrefs = false;
            _prefs = ConfigLoader.LoadClient(Path.Combine(_dataRoot, "client.json"));
        }

        var pose = _client.Host.PlayerPose;
        _camera.LookAt(pose.Position);
        var mouse = Mouse.GetState();
        var aim = _camera.ScreenToWorld(new Vector2(mouse.X, mouse.Y), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        _input.Sample(aim);
        _client.Host.Input = _input.Current;

        _accumulator += (float)gameTime.ElapsedGameTime.TotalSeconds;
        var fixedDt = 1f / _client.Host.TickRate;
        var steps = 0;
        while (_accumulator >= fixedDt && steps < MaxStepsPerFrame)
        {
            _client.Host.Step(fixedDt);
            _accumulator -= fixedDt;
            steps++;
        }

        if (steps == MaxStepsPerFrame)
            _accumulator = 0f;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_client is null || _prefs is null || _renderer is null || _overlay is null)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            return;
        }

        var pose = _client.Host.PlayerPose;
        _camera.LookAt(pose.Position);
        _renderer.Render(new RenderContext
        {
            PlayerPosition = pose.Position,
            PlayerFacingRadians = pose.FacingRadians,
            Colliders = _client.Host.Colliders,
        });
        _overlay.Draw(gameTime, _client.Host.TickIndex, Version, _prefs, _client.Host.Match, () => _reloadPrefs = true);
        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        _client?.Host.Dispose();
        _renderer?.Dispose();
        base.UnloadContent();
    }
}
