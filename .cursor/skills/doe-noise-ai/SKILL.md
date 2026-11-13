---
name: doe-noise-ai
description: duskOfEmpire noise, NPC AI, hordes, and PvPvE objectives. Use when implementing hearing/vision, FSM states, horde triggers on capture, sabotage, or enemy types (zombie, wailer, stalker, womb, herder, etc.).
---

# doe-noise-ai

Read `@docs/CONCEPT.md` §3, §10, §12, §14.

## Design north star

- **Noise kills** (or summons death). Movement, guns, constructions broadcast attraction.
- Players should feel hunted when loud; quiet play is a skill.

## Noise

- Loudness + duration → attraction radius.
- UI: direction + size ≈ proximity.
- Near walls: muffled hearing + echo/occlusion (approx OK in slice; tune in config).
- Breaking props quieter than gunshots.

## NPC perception

- Vision cone + hearing + alertness.
- Memory of last contact: go there, search, look around → calm (Wailer exception: aggro until source dead).
- States as needed: patrol / suspicious / search / combat / retreat — not every type needs all.

## PvPvE objectives

- Points are **shared**.
- Sabotage is intended gameplay.
- Horde waves **2 / 2 / 3** trigger on **capture start**.
- **No** soft-stop of the final push horde — pressure is the design.

## Priority

Implement enemy roster **early** (Phases 4–5), not as leftover polish. Prefer working FSMs over perfect animation.

## Config hooks

Keep womb/herder/horde/noise radii in `GameConfig` sections; avoid magic numbers in AI systems.

## Anti-patterns

- Silent meta that ignores noise pillar
- Per-team private objectives without CONCEPT change
- “Kill switch” that clears endgame horde for comfort
