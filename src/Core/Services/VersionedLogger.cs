using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Services;

public interface IVersionedLogger : ILogger
{
    string Version { get; }
}

public class VersionedFileLogger : FileLogger, IVersionedLogger
{
    public VersionedFileLogger(string logDir) : base(logDir) { }

    public string Version => $"{VersionInfo.ProductName} v{VersionInfo.Version} Build {VersionInfo.BuildDate}";
}