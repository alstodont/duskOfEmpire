---
name: doe-phase-dod
description: duskOfEmpire phase Definition of Done, scope cuts, and verification. Use at the start or end of a phase, when estimating cuts, or when an agent is about to expand scope past the current phase TZ.
---

# doe-phase-dod

## Before coding a phase

1. Open the phase TZ (`@docs/PHASE0.md` or later `docs/PHASEN.md`).
2. Paste/state: **Goal**, **DoD checklist**, **Explicit non-goals**.
3. Touch only folders needed for that phase.

## Phase map (reminder)

| Phase | Focus |
| --- | --- |
| 0 | Foundation, sim/render split, RT stubs, cube player, Windows |
| 1 | Map JSON, kinematic, city block primitives |
| 2 | Player walk/aim/sector/shoot ray/kick/weapon swap |
| 3 | Memory filter + phantoms + ImGui tune |
| 4–5 | Enemies ASAP |
| 6 | Weapons/elements |
| 7 | Inventory/loot rules |
| 8 | Constructions/noise |
| 9 | PvPvE objectives/hordes |
| 10 | LAN multiplayer |

## Cut order (if slipping)

1. Voice lines  
2. NPC polish  
3. Already-deferred procgen  

**Do not cut first:** memory truth model, noise as threat, shared sabotage objectives.

## Done means

- DoD boxes checked
- `dotnet build` OK on Windows target
- Manual 2-minute test notes written
- No silent scope creep into the next phase

## Agent output format at phase end

```text
Phase N: DONE | PARTIAL
Done: …
Not done: …
Verify: …
Next: …
```
