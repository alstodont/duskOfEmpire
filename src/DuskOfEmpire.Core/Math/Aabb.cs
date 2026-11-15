using System.Numerics;

namespace DuskOfEmpire.Core.Math;

public readonly struct Aabb
{
    public Aabb(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }

    public Vector2 Min { get; }
    public Vector2 Max { get; }

    public bool Overlaps(Aabb other) =>
        Min.X < other.Max.X && Max.X > other.Min.X &&
        Min.Y < other.Max.Y && Max.Y > other.Min.Y;

    public static Aabb FromCenter(Vector2 center, Vector2 half) =>
        new(center - half, center + half);
}
