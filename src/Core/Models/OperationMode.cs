namespace WindowsOptimizer.Core.Models;

public enum OperationMode
{
    Safe,
    Balanced,
    Strict
}

public class TweakResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class BackupInfo
{
    public string ComputerName { get; set; } = Environment.MachineName;
    public string OSVersion { get; set; } = Environment.OSVersion.ToString();
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Path { get; set; } = string.Empty;
}