using System.ServiceProcess;
using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface IServicesManager
{
    IEnumerable<TweakResult> Apply(OperationMode mode);
}

public class ServicesManager : IServicesManager
{
    private readonly ILogger _logger;

    public ServicesManager(ILogger logger)
    {
        _logger = logger;
    }

    public IEnumerable<TweakResult> Apply(OperationMode mode)
    {
        var results = new List<TweakResult>();
        var servicesToDisable = GetServicesForMode(mode);

        foreach (var svc in servicesToDisable)
        {
            try
            {
                using var service = new ServiceController(svc.Name);
                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
                // Note: permanent disable requires SCM changes, requires admin
                results.Add(new TweakResult { Success = true, Category = "Services", Name = svc.Name, Message = $"Stopped {svc.Name}" });
                _logger.Info($"Service {svc.Name} stopped");
            }
            catch (Exception ex)
            {
                results.Add(new TweakResult { Success = false, Category = "Services", Name = svc.Name, Message = ex.Message });
                _logger.Error($"Service {svc.Name} error: {ex.Message}");
            }
        }

        return results;
    }

    private List<(string Name, OperationMode MinMode)> GetServicesForMode(OperationMode mode)
    {
        var all = new List<(string, OperationMode)>
        {
            ("DiagTrack", OperationMode.Strict),
            ("WSearch", OperationMode.Balanced),
            ("SysMain", OperationMode.Balanced),
            ("BITS", OperationMode.Strict),
            ("Print Spooler", OperationMode.Balanced)
        };
        return all.Where(s => s.Item2 <= mode).Select(s => (s.Item1, s.Item2)).ToList();
    }
}