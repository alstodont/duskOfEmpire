namespace DuskOfEmpire.Core.Config;

public interface IClientPrefs
{
    float Volume { get; }
    float MouseSensitivity { get; }
    float Brightness { get; }
    int PostEffectQuality { get; }
}

// shortcut: these names stand in for the four categories until the glossary assigns fields
public sealed class ClientPrefs : IClientPrefs
{
    public float Volume { get; init; } = 1f;
    public float MouseSensitivity { get; init; } = 1f;
    public float Brightness { get; init; } = 1f;
    public int PostEffectQuality { get; init; } = 1;
}
