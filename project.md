# Windows Optimizer C# - Hybrid Edition

**Проект:** Windows Optimizer C#  
**Разработчик-проектировщик:** avk013  
**Исполнитель:** Cline mokoki  
**Версия:** 1.0.4  
**Дата сборки:** 2026-11-22

## Обзор

Гибридное приложение для оптимизации производительности Windows, вдохновленное Sofia Script for Windows. Предоставляет модульную архитектуру с поддержкой CLI и WPF GUI, предустановленными шаблонами оптимизации и безопасным применением изменений.

## Архитектура

### Core (net7.0-windows)
- **Models:** `OperationMode`, `WindowsInfo`, `VersionInfo`, `TweakResult`, `BackupInfo`
- **Services:**
  - `RegistryTweaksService` — твики реестра
  - `ServicesManager` — управление службами Windows
  - `StartupManager` — управление автозагрузкой
  - `VisualEffectsService` — визуальные эффекты
  - `PowerPlanService` — планы питания
  - `CleanupService` — очистка системы
  - `BackupService` — бэкап реестра по шаблону `ИмяПК_ОС_ГГГГММДД_ЧЧММСС`
  - `SafetyService` — защита от случайных изменений (PIN 111111)
  - `OptimizerEngine` — координация всех модулей
  - `VersionedFileLogger` — логирование с версионностью

### CLI (net7.0-windows)
Консольная оболочка для запуска оптимизаций из командной строки с указанием режима `Safe/Balanced/Strict`.

### GUI (WPF net7.0-windows)
Гибридный интерфейс управления:
- Выбор предустановленных профилей: Safe, Balanced, Strict, Gaming
- Создание/сохранение/удаление пользовательских шаблонов
- Предпросмотр параметров профиля до применения
- Безопасное применение с PIN-подтверждением (111111)
- Авторство отображается в интерфейсе

## Функциональность

1. **Модульная архитектура**
   - RegistryTweaks, ServicesManager, StartupManager, VisualEffects, PowerPlan, Cleanup

2. **Поддержка Windows 7/8/10/11**
   - Автоматическое определение версии ОС через WMI/реестр

3. **Режимы работы**
   - Safe — минимальные безопасные изменения
   - Balanced — оптимальный баланс производительности/стабильности
   - Strict — агрессивные твики для максимальной производительности

4. **Безопасность**
   - Бэкап реестра перед изменениями
   - PIN-подтверждение (111111) для системных действий
   - Предупреждения перед применением

5. **Шаблоны**
   - Предустановленные профили
   - Пользовательские JSON-профили
   - Сохранение/загрузка конфигураций

6. **Мультифреймворк**
   - Core независим от UI
   - Поддержка CLI и GUI
   - Легко расширяемый

## Сборка

```bash
# CLI
cd src/Cli
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true

# GUI
cd src/Gui
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
```

## Распространение

Архив с готовыми исполняемыми файлами:
`WindowsOptimizer-v1.0.4-Hybrid.7z`

Содержимое:
- `WindowsOptimizer.Cli.exe`
- `WindowsOptimizer.Gui.exe`
- `README.md`

## Требования

- Windows 7/8/10/11 x64
- .NET 7 Runtime (или self-contained)
- Права администратора для применения твиков

## Лицензия

Copyright (c) avk013. Project Architect: avk013. Implementation: Cline mokoki
