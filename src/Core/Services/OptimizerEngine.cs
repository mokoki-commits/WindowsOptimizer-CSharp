using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface IOptimizerEngine
{
    IEnumerable<TweakResult> Run(OperationMode mode, string pin);
}

public class OptimizerEngine : IOptimizerEngine
{
    private readonly ILogger _logger;
    private readonly IBackupService _backup;
    private readonly IRegistryTweaksService _registry;
    private readonly IServicesManager _services;
    private readonly IStartupManager _startup;
    private readonly IVisualEffectsService _visual;
    private readonly IPowerPlanService _power;
    private readonly ICleanupService _cleanup;

    public OptimizerEngine(ILogger logger, IBackupService backup, IRegistryTweaksService registry, IServicesManager services, IStartupManager startup, IVisualEffectsService visual, IPowerPlanService power, ICleanupService cleanup)
    {
        _logger = logger;
        _backup = backup;
        _registry = registry;
        _services = services;
        _startup = startup;
        _visual = visual;
        _power = power;
        _cleanup = cleanup;
    }

    private readonly ISafetyService _safety;

    public OptimizerEngine(ILogger logger, IBackupService backup, IRegistryTweaksService registry, IServicesManager services, IStartupManager startup, IVisualEffectsService visual, IPowerPlanService power, ICleanupService cleanup, ISafetyService safety)
    {
        _logger = logger;
        _backup = backup;
        _registry = registry;
        _services = services;
        _startup = startup;
        _visual = visual;
        _power = power;
        _cleanup = cleanup;
        _safety = safety;
    }

    public OptimizerEngine() : this(new VersionedFileLogger("Logs"), new BackupService(new VersionedFileLogger("Logs")), new RegistryTweaksService(new VersionedFileLogger("Logs"), WindowsInfoDetector.GetInfo()), new ServicesManager(new VersionedFileLogger("Logs")), new StartupManager(new VersionedFileLogger("Logs")), new VisualEffectsService(new VersionedFileLogger("Logs")), new PowerPlanService(new VersionedFileLogger("Logs")), new CleanupService(new VersionedFileLogger("Logs")), new SafetyService()) { }

    public IEnumerable<TweakResult> Run(OperationMode mode, string pin)
    {
        var results = new List<TweakResult>();
        var winInfo = WindowsInfoDetector.GetInfo();
        _logger.Info($"OS: {winInfo.VersionName} Build {winInfo.BuildNumber}");

        if (!_safety.ConfirmAction("Применение оптимизаций системы", pin))
        {
            _logger.Warn("Действия отменены пользователем из соображений безопасности");
            _safety.ReportProgress("PIN не подтвержден");
            results.Add(new TweakResult { Success = false, Category = "Safety", Name = "Confirm", Message = "Подтверждение отменено" });
            return results;
        }

        _safety.ReportProgress("Создание резервной копии...");
        var backupInfo = _backup.CreateRegistryBackup(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups"));
        _logger.Info($"Backup: {backupInfo.Path}");
        _safety.ReportProgress($"Бэкап создан: {backupInfo.Path}");

        _safety.ReportProgress("Применение твиков реестра...");
        results.AddRange(_registry.ApplyTweaks(mode));

        _safety.ReportProgress("Остановка служб...");
        results.AddRange(_services.Apply(mode));

        _safety.ReportProgress("Настройка автозапуска...");
        results.AddRange(_startup.Apply(mode));

        _safety.ReportProgress("Настройка визуальных эффектов...");
        results.AddRange(_visual.Apply(mode));

        _safety.ReportProgress("Настройка плана питания...");
        results.AddRange(_power.Apply(mode));

        _safety.ReportProgress("Очистка системы...");
        results.AddRange(_cleanup.Apply(mode));

        _safety.ReportProgress("Оптимизация завершена");
        return results;
    }
}