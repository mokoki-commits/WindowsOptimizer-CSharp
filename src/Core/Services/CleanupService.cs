using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface ICleanupService
{
    IEnumerable<TweakResult> Apply(OperationMode mode);
}

public class CleanupService : ICleanupService
{
    private readonly ILogger _logger;

    public CleanupService(ILogger logger)
    {
        _logger = logger;
    }

    public IEnumerable<TweakResult> Apply(OperationMode mode)
    {
        var results = new List<TweakResult>();
        // Placeholder for temp files cleanup
        _logger.Info("Cleanup: temp files");
        results.Add(new TweakResult { Success = true, Category = "Cleanup", Name = "TempFiles", Message = "Cleaned" });
        return results;
    }
}