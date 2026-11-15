using System.Text.Json;

namespace DuskOfEmpire.Core.Config;

public static class ConfigLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    public static GameConfig LoadMatch(string path)
    {
        var config = JsonSerializer.Deserialize<GameConfig>(File.ReadAllText(path), Options)
            ?? throw new InvalidDataException(path);
        if (config.Memory is null || config.Pvpve is null || config.Womb is null
            || config.Herder is null || config.Item is null || config.Slots is null
            || config.Loot is null)
            throw new InvalidDataException($"{path} is missing a section");
        return config;
    }

    public static ClientPrefs LoadClient(string path) =>
        JsonSerializer.Deserialize<ClientPrefs>(File.ReadAllText(path), Options)
        ?? throw new InvalidDataException(path);
}
