# duskOfEmpire

Техномагический постапокалиптический top-down шутер с ограниченным восприятием (память / сектор / шум). PvPvE.

## Документы

- [Концепт v1.1](docs/CONCEPT.md) — геймдизайн и архитектурные locks
- [ТЗ Phase 0](docs/PHASE0.md) — фундамент MonoGame / sim·render / host·client
- [Движок](docs/ENGINE.md) — контракт шага, авторитета и кадра
- [AGENTS.md](AGENTS.md) — brief для Cursor-агентов

## Сборка (Windows)

Нужен .NET 8 SDK.

```
dotnet build DuskOfEmpire.sln
dotnet run --project src/DuskOfEmpire.Client
```

Двойной щелчок по `run-client.bat` или по ярлыку «duskOfEmpire» на рабочем столе делает то же самое и сам подставляет SDK из `%USERPROFILE%\.dotnet`.

Окно 1920×1080. WASD двигает куб в плоскости, мышь задаёт направление, Esc закрывает окно. Стены арены держат куб внутри кадра.

`data/config.json` заполняет правила матча. `data/client.json` хранит громкость, чувствительность мыши, яркость и качество постэффектов. Кнопка в окне ImGui перечитывает только `client.json` рядом с запущенным exe (`bin/Debug/net8.0/data/client.json`). Правила матча эта кнопка не меняет.

## Стек

MonoGame DesktopGL · Arch ECS · своя kinematic-физика · LiteNetLib (LAN) · HLSL (memory + light RT)
