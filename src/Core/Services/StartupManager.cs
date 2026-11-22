using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface IStartupManager
{
    IEnumerable<TweakResult> Apply(OperationMode mode);
}

public class StartupManager : IStartupManager
{
    private readonly ILogger _logger;

    public StartupManager(ILogger logger)
    {
        _logger = logger;
    }

    public IEnumerable<TweakResult> Apply(OperationMode mode)
    {
        _logger.Info("StartupManager: scan startup items");
        // Placeholder: real implementation would query HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run and Task Scheduler
        yield return new TweakResult { Success = true, Category = "Startup", Name = "Scan", Message = "Startup items scanned" };
    }
}