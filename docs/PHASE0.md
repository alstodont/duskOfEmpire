# ТЕХЗАДАНИЕ — PHASE 0 (Фундамент) — duskOfEmpire

Связано с [CONCEPT.md](./CONCEPT.md) v1.1.

## P0.0. Цель

Рабочий MonoGame-проект: пустая ortho-сцена, каркас **sim/render** и **host/client**, задел под **light** и **memory** RT, дебаг ImGui. Геймплея почти нет — только куб-игрок и фундамент.

## P0.1. Definition of Done

- Солюшен собирается на **Windows** из Rider или `dotnet build` / `dotnet run`.
- Окно 1920×1080, пустая сцена, ortho top-down камера.
- ImGui: FPS, версия, sim tick (30 Hz), кнопка reload **клиентского** конфига.
- `config.json` загружается; секции памяти/PvPvE/NPC/слотов доступны в `GameConfig`.
- Пустой валидный `data/maps/test_city.json`.
- Игрок — куб: WASD + поворот за мышью (без сектора/стрельбы).
- Есть пустые/заглушечные RT: `sceneRT`, `memoryMaskRT`, `memoryViewRT`, `lightRT`.
- Структура папок и проекты соответствуют §P0.3.
- Linux/Mac **не** входят в DoD Phase 0.

## P0.2. Пакеты (NuGet)

```
MonoGame.Framework.DesktopGL     3.8.*
MonoGame.Content.Builder.Task    3.8.*
Arch                             2.*
MonoGame.ImGuiNet                * (совместимая с MonoGame 3.8)
ImGui.NET                        *
LiteNetLib                       *
MemoryPack                       *
MemoryPack.Generator             *
System.Text.Json                 (inbox .NET 8)
```

## P0.3. Структура солюшена

```
DuskOfEmpire.sln
├── src/
│   ├── DuskOfEmpire.Core/                 // без MonoGame
│   │   ├── Config/
│   │   ├── Math/
│   │   ├── Ecs/
│   │   ├── Map/
│   │   └── Sim/                           // интерфейсы симуляции, тик, команды ввода
│   │
│   ├── DuskOfEmpire.Sim/                  // чистая симуляция (host)
│   │   ├── Systems/                       // MovementSystem, ... — только Update
│   │   └── HostWorld.cs
│   │
│   ├── DuskOfEmpire.Rendering/            // MonoGame
│   │   ├── Camera2D.cs
│   │   ├── PrimitivesRenderer.cs
│   │   ├── RenderTargets/                 // scene, memoryMask, memoryView, light
│   │   └── Effects/                       // memory.fx, light.fx — заглушки
│   │
│   ├── DuskOfEmpire.Client/               // точка входа Game + view
│   │   ├── Program.cs
│   │   ├── Game1.cs
│   │   ├── ClientWorld.cs                 // предикт, локальная память (позже)
│   │   └── Content/
│   │
│   ├── DuskOfEmpire.Gameplay/             // player input binding, shared gameplay types
│   ├── DuskOfEmpire.Physics/
│   ├── DuskOfEmpire.AI/                   // пусто
│   ├── DuskOfEmpire.Perception/           // пусто (память — Phase 3)
│   ├── DuskOfEmpire.Net/                  // каркас LiteNetLib, пока без геймплея
│   └── DuskOfEmpire.Debug/                // ImGui overlays
│
├── data/
│   ├── config.json
│   └── maps/test_city.json
│
└── tools/
    └── map_inspector/
```

Принцип: **Sim-системы не ссылаются на Rendering**. Client может ссылаться на Sim-DTO/команды, но не тащит host-only логику в draw.

## P0.4. Ключевые контракты

### Тик

```
public interface ISimClock
{
    float TickRate { get; }      // 30
    int TickIndex { get; }
    void Step(float fixedDt);    // 1/30
}
```

### Системы

```
public interface ISimSystem
{
    void Update(in SimContext ctx);
}

public interface IRenderSystem
{
    void Render(in RenderContext ctx);
}
```

`ISystem` с `Update`+`Render` **не используем**.

### Config

```
public sealed class GameConfig
{
    public MemorySection Memory { get; init; } = new();
    public PvpveSection Pvpve { get; init; } = new();
    public WombSection Womb { get; init; } = new();
    public HerderSection Herder { get; init; } = new();
    public ItemSection Item { get; init; } = new();
    public SlotSection Slots { get; init; } = new();
    public LootSection Loot { get; init; } = new();
    public int FriendlyFirePct { get; init; } = 10;
    public int RespawnPvpveSec { get; init; } = 30;
}

public sealed class LootSection
{
    public float GroundCleanupSec { get; init; } = 300;      // 5 мин
    public float TopLootWindowSec { get; init; } = 600;      // last 10 мин
    public float ShooterConsumableCooldownSec { get; init; } = 30;
}
```

Разделение hot-reload:

| Категория | Примеры | Hot-reload в матче |
| --- | --- | --- |
| Match rules (host) | Memory*, урон, орды, FF, loot timers | только хост → репликация |
| Client prefs | громкость, чувствительность, яркость | локально |

\* В Phase 0 сети ещё нет — достаточно API `IMatchConfig` vs `IClientPrefs` и пометки в JSON/коде.

### Map / Collider / Input

Как в v1.0 (`IMapLoader`, `MapData`, `ICollider`, `IInputSource`), namespaces `DuskOfEmpire.*`.

## P0.5. Checklist

1. `dotnet new sln -n DuskOfEmpire`
2. Создать проекты §P0.3
3. NuGet §P0.2
4. `DuskOfEmpire.Client` — DesktopGL Game, 1920×1080
5. `ISimClock` 30 Hz + accumulator в `Game1`
6. Загрузка `config.json` / `test_city.json`
7. `PrimitivesRenderer` + `Camera2D`
8. Куб-игрок: WASD, aim mouse (sim movement + render отдельными системами)
9. AABB collision + spatial hash (минимум)
10. RT-заглушки: scene / memoryMask / memoryView / **light**
11. `.fx` stubs: `memory.fx`, `light.fx` через MGCB
12. ImGui: FPS, tick, version, reload client prefs / full config out-of-match
13. Каркас `DuskOfEmpire.Net` (create host/client sockets API, без геймплея)
14. README: сборка/запуск **Windows**
15. Скриншот + тег `v0.1-phase0-done`

## P0.6. НЕ делаем в Phase 0

- Память/фантомы (только RT + fx stub)
- Стрельба, NPC, сектор
- Реальный LAN-матч (только каркас Net)
- Модели кроме примитивов
- Linux CI как требование

## P0.7. Артефакты

- Рабочий солюшен `DuskOfEmpire.sln`
- `README.md` (Windows)
- `data/config.json` с константами v1.1
- `data/maps/test_city.json`
- Скриншот с ImGui
- Git tag `v0.1-phase0-done`

## P0.8. Открытые вопросы к Phase 1

- Размер тайла/игрока/стен в метрах (числовые).
- Кто автор `test_city.json` (ручной JSON / инструмент).
- Texture atlas later; Phase 1 — solid colors + light stubs ok.
