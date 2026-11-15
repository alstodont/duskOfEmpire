using System.Numerics;
using DuskOfEmpire.Core.Math;
using DuskOfEmpire.Core.Sim;

namespace DuskOfEmpire.Physics;

public sealed class SpatialCollision : ICollisionWorld
{
    private readonly float _cell;
    private readonly Dictionary<long, List<int>> _cells = new();
    private readonly Dictionary<int, Aabb> _boxes = new();
    private readonly List<int> _hits = new();
    private int _nextId;

    static SpatialCollision()
    {
        var world = new SpatialCollision(2f);
        world.Add(new Aabb(new Vector2(0f, 0f), new Vector2(1f, 1f)));
        var blocked = world.Move(new Vector2(-1f, 0.5f), new Vector2(0.4f, 0.4f), new Vector2(2f, 0f));
        if (blocked.X != -1f || blocked.Y != 0.5f)
            throw new InvalidOperationException("spatial hash rejected a blocked move");
        var free = world.Move(new Vector2(-2f, 0.5f), new Vector2(0.2f, 0.2f), new Vector2(0.5f, 0f));
        if (MathF.Abs(free.X - (-1.5f)) > 0.0001f || MathF.Abs(free.Y - 0.5f) > 0.0001f)
            throw new InvalidOperationException("spatial hash rejected a free move");
    }

    public SpatialCollision(float cellSize) => _cell = cellSize;

    public void Add(Aabb box)
    {
        var id = _nextId++;
        _boxes.Add(id, box);
        var x0 = Cell(box.Min.X);
        var y0 = Cell(box.Min.Y);
        var x1 = Cell(box.Max.X);
        var y1 = Cell(box.Max.Y);
        for (var y = y0; y <= y1; y++)
        {
            for (var x = x0; x <= x1; x++)
            {
                var key = Key(x, y);
                if (!_cells.TryGetValue(key, out var list))
                {
                    list = new List<int>();
                    _cells.Add(key, list);
                }

                list.Add(id);
            }
        }
    }

    public Vector2 Move(Vector2 position, Vector2 halfExtents, Vector2 delta)
    {
        position = Slide(position, halfExtents, new Vector2(delta.X, 0f));
        return Slide(position, halfExtents, new Vector2(0f, delta.Y));
    }

    private Vector2 Slide(Vector2 position, Vector2 halfExtents, Vector2 delta)
    {
        if (delta.X == 0f && delta.Y == 0f)
            return position;
        var next = position + delta;
        Query(Aabb.FromCenter(next, halfExtents), _hits);
        return _hits.Count == 0 ? next : position;
    }

    private void Query(Aabb box, List<int> hits)
    {
        hits.Clear();
        var x0 = Cell(box.Min.X);
        var y0 = Cell(box.Min.Y);
        var x1 = Cell(box.Max.X);
        var y1 = Cell(box.Max.Y);
        for (var y = y0; y <= y1; y++)
        {
            for (var x = x0; x <= x1; x++)
            {
                if (!_cells.TryGetValue(Key(x, y), out var list))
                    continue;
                foreach (var id in list)
                {
                    if (hits.Contains(id))
                        continue;
                    if (_boxes[id].Overlaps(box))
                        hits.Add(id);
                }
            }
        }
    }

    private int Cell(float value) => (int)MathF.Floor(value / _cell);

    private static long Key(int x, int y) => ((long)x << 32) | (uint)y;
}
