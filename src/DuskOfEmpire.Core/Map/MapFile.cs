using DuskOfEmpire.Core.Math;

namespace DuskOfEmpire.Core.Map;

public interface ICollider
{
    Aabb Bounds { get; }
}

public sealed class BoxCollider : ICollider
{
    public BoxCollider(Aabb bounds) => Bounds = bounds;

    public Aabb Bounds { get; }
}

public sealed class MapData
{
    public IReadOnlyList<ICollider> Colliders { get; init; } = Array.Empty<ICollider>();
}

public interface IMapLoader
{
    MapData Load(string path);
}

public sealed class JsonMapLoader : IMapLoader
{
    public MapData Load(string path)
    {
        using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
        if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Object)
            throw new InvalidDataException($"{path} must be a JSON object");
        // shortcut: collider fields arrive with the map contract in Phase 1
        return new MapData();
    }
}
