# duskOfEmpire — agent brief

Before coding, read:

1. `docs/CONCEPT.md` (v1.1 locks)
2. `docs/PHASE0.md` (or later phase TZ)
3. Matching skill under `.cursor/skills/` if the task touches that domain

## Skills

| Skill | Use when |
| --- | --- |
| `doe-architecture` | projects, sim/render, tick, config authority, Phase 0 structure |
| `doe-perception` | sector, memory fog, phantoms, minimap explore, light RT |
| `doe-noise-ai` | NPC FSM, hearing/vision, hordes, objectives/sabotage |
| `doe-net-lan` | LiteNetLib host/client, snapshots, hitscan authority |
| `doe-phase-dod` | starting/finishing a phase; scope cuts |
| `doe-prompt-templates` | need a ready prompt for a role/phase |

## Default workflow

1. State phase + DoD + out-of-scope.
2. Implement minimal change.
3. `dotnet build` (Windows DesktopGL).
4. Note manual 2-minute verification steps.
