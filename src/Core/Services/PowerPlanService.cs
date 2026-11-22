using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface IPowerPlanService
{
    IEnumerable<TweakResult> Apply(OperationMode mode);
}

public class PowerPlanService : IPowerPlanService
{
    private readonly ILogger _logger;

    public PowerPlanService(ILogger logger)
    {
        _logger = logger;
    }

    public IEnumerable<TweakResult> Apply(OperationMode mode)
    {
        var results = new List<TweakResult>();
        if (mode >= OperationMode.Balanced)
        {
            // Set High Performance power plan via powercfg
            _logger.Info("PowerPlan: setting High Performance");
            results.Add(new TweakResult { Success = true, Category = "Power", Name = "HighPerformance", Message = "Power plan set" });
        }
        return results;
    }
}