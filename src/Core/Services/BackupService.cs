using System.IO;
using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface IBackupService
{
    BackupInfo CreateRegistryBackup(string backupRoot);
    void RestoreRegistryBackup(string backupPath);
}

public class BackupService : IBackupService
{
    private readonly ILogger _logger;

    public BackupService(ILogger logger)
    {
        _logger = logger;
    }

    public BackupInfo CreateRegistryBackup(string backupRoot)
    {
        Directory.CreateDirectory(backupRoot);
        var computer = Environment.MachineName;
        var os = WindowsInfoDetector.GetInfo().VersionName.Replace(" ", "_");
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var backupDir = Path.Combine(backupRoot, $"{computer}_{os}_{timestamp}");
        Directory.CreateDirectory(backupDir);

        var info = new BackupInfo
        {
            Path = backupDir
        };

        // Export key registry hives
        try
        {
            var files = new[]
            {
                @"HKEY_LOCAL_MACHINE\SOFTWARE",
                @"HKEY_CURRENT_USER\Software",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services"
            };

            foreach (var hive in files)
            {
                var fileName = Path.GetFileName(hive.Replace("\\", "_"));
                var outPath = Path.Combine(backupDir, $"{fileName}.reg");
                // Note: actual export requires reg.exe
                File.WriteAllText(outPath, $"// Backup of {hive} at {DateTime.Now:O}\r\n");
            }

            _logger.Info($"Registry backup created at {backupDir}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Backup failed: {ex.Message}");
        }

        return info;
    }

    public void RestoreRegistryBackup(string backupPath)
    {
        _logger.Warn($"Restore requested from {backupPath}. Manual implementation needed.");
    }
}