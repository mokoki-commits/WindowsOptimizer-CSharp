using Microsoft.Win32;
using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface IRegistryTweaksService
{
    IEnumerable<TweakResult> ApplyTweaks(OperationMode mode);
}

public class RegistryTweaksService : IRegistryTweaksService
{
    private readonly ILogger _logger;
    private readonly WindowsInfo _winInfo;

    public RegistryTweaksService(ILogger logger, WindowsInfo winInfo)
    {
        _logger = logger;
        _winInfo = winInfo;
    }

    public IEnumerable<TweakResult> ApplyTweaks(OperationMode mode)
    {
        var results = new List<TweakResult>();

        // Disable Last Access Time
        results.Add(SetRegistry(@"SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisableLastAccessUpdate", 1, "Disable NTFS Last Access Time"));

        // Disable 8.3 name creation
        results.Add(SetRegistry(@"SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 1, "Disable 8.3 Names"));

        if (mode >= OperationMode.Balanced)
        {
            // Disable Prefetch
            results.Add(SetRegistry(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnablePrefetcher", 0, "Disable Prefetcher"));
            results.Add(SetRegistry(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters", "EnableSuperfetch", 0, "Disable Superfetch"));
        }

        if (mode >= OperationMode.Strict)
        {
            // Disable telemetry
            results.Add(SetRegistry(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection", "AllowTelemetry", 0, "Disable Telemetry"));
            results.Add(SetRegistry(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", "AllowTelemetry", 0, "Disable Telemetry User"));
        }

        return results;
    }

    private TweakResult SetRegistry(string subKey, string valueName, object value, string description)
    {
        try
        {
            using var key = Registry.LocalMachine.CreateSubKey(subKey);
            if (key != null)
            {
                var currentValue = key.GetValue(valueName)?.ToString();
                if (currentValue?.ToString() == value.ToString())
                {
                    return new TweakResult { Success = true, Category = "Registry", Name = description, Message = "Already set" };
                }

                key.SetValue(valueName, value, RegistryValueKind.DWord);
                _logger.Info($"Registry: {description} set (Old: {currentValue}, New: {value})");
                return new TweakResult { Success = true, Category = "Registry", Name = description, Message = $"Changed (Old: {currentValue}, New: {value})", OldValue = currentValue, NewValue = value.ToString() };
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Registry error {description}: {ex.Message}");
        }
        return new TweakResult { Success = false, Category = "Registry", Name = description, Message = "Failed" };
    }
}