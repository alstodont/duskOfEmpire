---
name: doe-perception
description: duskOfEmpire perception — view sector truth, memory fog filter on map skeleton, client phantoms (mimic/no-op), ally presence blot, explore minimap, light RT. Use for memory shaders, sector cone, phantoms, minimap, lighting pipeline.
---

# doe-perception

Source of truth: `@docs/CONCEPT.md` §4, §15, §16 and locks in v1.1.

## Truth model

- **Current sector = truth.** No phantoms, false loot, or memory distortion inside it.
- Outside sector = **per-client memory**. Independent for every local view (LAN 2–4).
- Shooting only into current sector.
- Ally outside sector → colored blot = **presence only** (no class/HP/facing/status).

## Memory visuals

- Style reference: Darkwood fog / desaturation / soft blur.
- Implement as a **filter on map skeleton + explored/stale mask**, not “blur the whole framebuffer for beauty.”
- Stages from config (`Clear`…`Gone`); class modifier ±20s; re-entering sector refreshes instantly.
- False memory must look like real memory.

## Phantoms

- Spawn/update **only on client**; never as host ECS combat entities.
- May visually mimic anything (chase, swing, “shoot”).
- Server effect: **none**. Hits/damage/blocking are no-ops.
- Entering true sector → destroy same frame.

## Render targets

Expected pipeline slots (even as stubs):

- `sceneRT`
- `memoryMaskRT` (often R32 / mask)
- `memoryViewRT`
- `lightRT` — lighting is a first-class engine mechanic; keep the slot from Phase 0

## Minimap

- Art direction: hand-drawn **style**.
- Data: fog-of-war / explored path — draw where the local player has been.

## Anti-patterns

- Server-authoritative phantoms
- Shared global memory texture for all players
- Fullscreen blur that destroys combat readability
- Ally ESP via blot extras
