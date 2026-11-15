using ImGuiNET;
using Microsoft.Xna.Framework;
using Monogame.Imgui.Renderer;
using DuskOfEmpire.Core.Config;

namespace DuskOfEmpire.Debug;

public sealed class DebugOverlay
{
    private readonly Game _game;
    private readonly ImGuiRenderer _imgui;

    public DebugOverlay(Game game)
    {
        _game = game;
        _imgui = new ImGuiRenderer(game);
        _imgui.RebuildFontAtlas();
    }

    public void Draw(GameTime time, int tick, string version, IClientPrefs prefs, IMatchConfig match, Action reloadClientPrefs)
    {
        _imgui.BeforeLayout(_game, time);
        ImGui.Begin("duskOfEmpire");
        ImGui.Text($"FPS {ImGui.GetIO().Framerate:0}");
        ImGui.Text($"version {version}");
        ImGui.Text($"tick {tick} @ 30 Hz");
        ImGui.Text($"friendly fire {match.FriendlyFirePct}%");
        ImGui.Text($"volume {prefs.Volume:0.00}");
        ImGui.Text($"mouse {prefs.MouseSensitivity:0.00}");
        ImGui.Text($"brightness {prefs.Brightness:0.00}");
        ImGui.Text($"post {prefs.PostEffectQuality}");
        if (ImGui.Button("reload client prefs"))
            reloadClientPrefs();
        ImGui.End();
        _imgui.AfterLayout();
    }
}
