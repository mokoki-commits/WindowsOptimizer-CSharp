using Microsoft.Win32;
using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface IVisualEffectsService
{
    IEnumerable<TweakResult> Apply(OperationMode mode);
}

public class VisualEffectsService : IVisualEffectsService
{
    private readonly ILogger _logger;

    public VisualEffectsService(ILogger logger)
    {
        _logger = logger;
    }

    public IEnumerable<TweakResult> Apply(OperationMode mode)
    {
        var results = new List<TweakResult>();
        if (mode >= OperationMode.Balanced)
        {
            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects");
                key?.SetValue("VisualFXSetting", 2); // 2 = Best performance
                results.Add(new TweakResult { Success = true, Category = "Visual", Name = "VisualEffects", Message = "Set to Best Performance" });
                _logger.Info("Visual effects set to best performance");
            }
            catch (Exception ex)
            {
                results.Add(new TweakResult { Success = false, Category = "Visual", Name = "VisualEffects", Message = ex.Message });
            }
        }
        return results;
    }
}