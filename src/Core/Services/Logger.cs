using System.IO;
using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface ILogger
{
    bool IsEnabled { get; set; }

    void Info(string message);
    void Warn(string message);
    void Error(string message);
    void Log(TweakResult result);
}

public class FileLogger : ILogger
{
    private readonly string _logPath;
    private readonly object _lock = new();

    public FileLogger(string logDir)
    {
        Directory.CreateDirectory(logDir);
        _logPath = Path.Combine(logDir, $"optimizer_{DateTime.Now:yyyyMMdd_HHmmss}.log");
    }

    public bool IsEnabled { get; set; } = true;
    public void Info(string message) { if (IsEnabled) Write("INFO", message); }
    public void Warn(string message) { if (IsEnabled) Write("WARN", message); }
    public void Error(string message) { if (IsEnabled) Write("ERROR", message); }

    public void Log(TweakResult result)
    {
        if (!IsEnabled) return;
        var line = $"{DateTime.Now:O} [{result.Category}/{result.Name}] Success:{result.Success} - {result.Message}";
        Write(result.Success ? "OK" : "FAIL", line);
    }

    private void Write(string level, string message)
    {
        lock (_lock)
        {
            var line = $"{DateTime.Now:O} [{level}] {message}";
            Console.WriteLine(line);
            File.AppendAllText(_logPath, line + Environment.NewLine);
        }
    }

    public string LogPath => _logPath;
}