# duskOfEmpire

Техномагический постапокалиптический top-down шутер с ограниченным восприятием (память / сектор / шум). PvPvE.

## Документы

- [Концепт v1.1](docs/CONCEPT.md) — геймдизайн и архитектурные locks
- [ТЗ Phase 0](docs/PHASE0.md) — фундамент MonoGame / sim·render / host·client
- [AGENTS.md](AGENTS.md) — brief для Cursor-агентов

## Cursor

- Rules: `.cursor/rules/` (`duskofempire-core`, `csharp-monogame`)
- Skills: `.cursor/skills/doe-*` (architecture, perception, noise-ai, net-lan, phase-dod, prompt-templates)

## Статус

Репозиторий на этапе документации + agent tooling. Реализация стартует с Phase 0 (Windows, MonoGame 3.8, .NET 8).

## Стек (кратко)

MonoGame DesktopGL · Arch ECS · своя kinematic-физика · LiteNetLib (LAN) · HLSL (memory + light RT)
