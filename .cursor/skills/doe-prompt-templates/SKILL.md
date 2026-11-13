---
name: doe-prompt-templates
description: Ready prompt templates for duskOfEmpire agent roles and phases. Use when starting a new chat, switching roles, requesting a design critique, net review, or shader task.
---

# doe-prompt-templates

Copy a block into a new agent chat. Keep **one role + one phase** per chat.

## Bootstrap (any task)

```text
Контекст: docs/CONCEPT.md v1.1 + docs/PHASE0.md (или PHASE N).
Locks: DuskOfEmpire, sim≠render, host 30Hz, память клиентская независимая,
фантомы mimick/no-op, Windows, LAN.
Задача: <одна>. Не трогай вне скоупа.
Сначала план файлов, потом код, потом dotnet build.
В конце — как проверить руками за 2 минуты.
```

## Architect (Phase 0 / structure)

```text
Роль: архитектор duskOfEmpire.
Сделай только каркас Phase 0 по docs/PHASE0.md:
проекты DuskOfEmpire.*, ISimSystem/IRenderSystem, ISimClock 30Hz,
RT stubs scene/memoryMask/memoryView/light, Net stub.
Без геймплея оружия/NPC/памяти. Windows DoD only.
```

## Perception / GFX

```text
Роль: perception/GFX.
Память = фильтр тумана/десата на каркас карты (Darkwood-like), не beauty fullscreen blur.
Фантомы только клиент, полный mimick, нулевой эффект.
Союзник вне сектора = только факт присутствия.
Свет: сохрани/используй lightRT. Читаемость боя > красота.
```

## AI / noise

```text
Роль: AI + noise.
Шум = смерть. Слух/зрение NPC, FSM.
PvPvE: общие точки, саботаж нужен, орды 2/2/3 на старт захвата,
у финиша орду не отключать. Константы — в config.json.
```

## Net / LAN

```text
Роль: network.
LiteNetLib listen-host, host authoritative hitscan/NPC/loot/objectives.
Клиент: предикт только себя + своя память.
Клиентская память/фантомы не дают серверный эффект.
Найди trust boundaries и закрой дыры на host-path.
```

## Pillar critique (no code)

```text
Не пиши код. Оцени текущий дифф/поведение:
усиливает ли (1) шум → смерть (2) правда только в секторе?
Если нет — минимальный патч-план.
```

## Scope cutter

```text
Фаза плывёт. Предложи cut-list по порядку CONCEPT
(голос → polish NPC → procgen). Не предлагай резать память/шум/общие точки.
```

## Balance (config only)

```text
Танк-дробовик vs стрелок (random consumable 30с).
Орды 2/2/3, лут: top window last 10 min, cleanup 5 min.
Дай ≤5 правок полей config.json и ожидаемый feel каждой.
```
