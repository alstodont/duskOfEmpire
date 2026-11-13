---
name: doe-architecture
description: duskOfEmpire architecture — Sim vs Render, host 30 Hz, DuskOfEmpire.* projects, match vs client config. Use when scaffolding solutions, tick loops, ECS boundaries, Phase 0 structure, or debating where code belongs.
---

# doe-architecture

Read `@docs/CONCEPT.md` §17 and `@docs/PHASE0.md` before changing structure.

## Hard boundaries

| Layer | May depend on | Must not |
| --- | --- | --- |
| `DuskOfEmpire.Core` | nothing game-engine | MonoGame, LiteNetLib types in public API if avoidable |
| `DuskOfEmpire.Sim` | Core | Rendering, ImGui, content loaders that need GraphicsDevice |
| `DuskOfEmpire.Rendering` | Core (+ MonoGame) | mutating authoritative world state |
| `DuskOfEmpire.Client` | Sim DTOs/commands, Rendering, Net | owning “truth” for remote entities |
| `DuskOfEmpire.Net` | Core DTOs | drawing |

## Tick

- Host sim: fixed **30 Hz** (`dt = 1/30`), accumulator in client/host loop.
- `ISimClock` exposes tick index for net/replay.
- Render interpolates; do not step gameplay from Draw.

## Interfaces

Prefer:

```csharp
ISimSystem { void Update(in SimContext ctx); }
IRenderSystem { void Render(in RenderContext ctx); }
```

Do **not** revive a combined `ISystem` with both Update and Render.

## Config authority

- `IMatchConfig` / match sections: host loads, replicates; clients treat as read-only in-match.
- `IClientPrefs`: local; ImGui reload OK anytime.
- Phase 0 without net: still **split types/API** so Phase 10 does not rewrite config.

## Checklist before merging architecture work

- [ ] No sim project reference to Rendering
- [ ] No gameplay authority computed only on a non-host client path
- [ ] RT stubs include light + memory slots if touching render bootstrap
- [ ] Namespaces are `DuskOfEmpire.*`
