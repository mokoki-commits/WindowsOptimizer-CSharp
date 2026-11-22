# GitHub Publish Guide

## Версионность

Проект использует версионность в нескольких местах:
- `VersionInfo.cs` - константы версии, продукта, даты сборки
- `csproj` - `<Version>`, `<FileVersion>`, `<AssemblyVersion>`, `<InformationalVersion>`

Текущая версия: **1.0.0** Build 2026-11-22

Обновление версии:
1. Изменить `VersionInfo.Version` и `BuildDate`
2. Обновить `Version`, `FileVersion`, `AssemblyVersion` в проектах
3. Обновить git tag

## Сборка для публикаций

```bash
cd src/Cli
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o ../publish
```

## Архив для тестирования

Сборка опубликована в архив:
`/home/testai26/.cline/data/workspaces/chat/WindowsOptimizer-CSharp/WindowsOptimizer-v1.0.0.tar.gz`

Содержимое архива:
- WindowsOptimizer.Cli.exe - single file exe
- WindowsOptimizer.Cli.pdb
- README.md

## GitHub Release

1. Создать tag: `git tag -a v1.0.0 -m "Release 1.0.0"`
2. Push tag: `git push origin v1.0.0`
3. Создать Release на GitHub с assets:
   - WindowsOptimizer-v1.0.0.tar.gz
   - WindowsOptimizer-v1.0.0.zip (если нужен)
4. Обновить CHANGELOG.md

## Структура проекта

- `src/Core` - мультифреймворковое ядро, независимое от UI
- `src/Cli` - консольная оболочка
- `src/Gui` - заготовка для WPF/WinUI

Бэкапы создаются по шаблону: `ИмяПК_ОС_ГГГГММДД_ЧЧММСС`
