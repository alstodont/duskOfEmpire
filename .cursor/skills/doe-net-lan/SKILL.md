---
name: doe-net-lan
description: duskOfEmpire LAN networking — LiteNetLib listen-host, 30 Hz authority, hitscan lag compensation, client prediction, MemoryPack snapshots. Use for Net project, multiplayer sync, cheating surface around client memory.
---

# doe-net-lan

Read `@docs/CONCEPT.md` §17 architecture locks and `@docs/PHASE0.md` Net stub notes.

## Topology

- Slice multiplayer: **LAN same room**, one listen-host + clients (separate processes/machines).
- Not split-screen. Not required dedicated server binary in Phase 0 — but **API split** host vs client from day one.

## Authority

| Concern | Owner |
| --- | --- |
| World positions (remotes, NPC) | Host |
| Hitscan / damage / status | Host (+ lag compensation) |
| Loot spawn/pickup/cleanup | Host |
| Objectives / sabotage / horde triggers | Host |
| Local movement prediction | Client (reconcile to host) |
| Memory fog, phantoms, local light view | Client only |

## Transport

- LiteNetLib.
- Snapshots / RPCs: MemoryPack.
- Sim tick 30 Hz; send rates may be lower but state must be tick-keyed.

## Projectile model

- Visual streak on clients.
- Hit resolution: **server hitscan** with lag compensation.
- Homing/ricochet flags later — still host-validated.

## Security mindset

Client memory/phantoms must never grant:

- damage
- blocking
- loot
- objective progress

If a client “hits” a phantom, host ignores. If unsure, put the check on host.

## Phase 0 expectation

Wire package + create host/client socket scaffolding only. No full match loop until Phase 10 — but do not paint the architecture into a singleplayer corner.

## Anti-patterns

- Each client simulating NPC combat as truth
- Trusting client “I shot entity X for Y damage”
- One shared memory RT reused as if all players share vision
