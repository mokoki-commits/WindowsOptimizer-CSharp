namespace WindowsOptimizer.Core.Models;

public class WindowsInfo
{
    public int MajorVersion { get; set; }
    public int MinorVersion { get; set; }
    public int BuildNumber { get; set; }
    public string VersionName { get; set; } = "Unknown";
    public string Edition { get; set; } = "Unknown";
    public bool IsWindows7 => MajorVersion == 6 && MinorVersion == 1;
    public bool IsWindows8 => MajorVersion == 6 && MinorVersion == 2;
    public bool IsWindows10 => MajorVersion == 10 && MinorVersion == 0;
    public bool IsWindows11 => MajorVersion == 10 && MinorVersion == 0 && BuildNumber >= 22000;
}

public static class WindowsInfoDetector
{
    public static WindowsInfo GetInfo()
    {
        var info = new WindowsInfo
        {
            MajorVersion = Environment.OSVersion.Version.Major,
            MinorVersion = Environment.OSVersion.Version.Minor,
            BuildNumber = Environment.OSVersion.Version.Build
        };

        // Try to get more info via registry
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (key != null)
            {
                var productName = key.GetValue("ProductName")?.ToString();
                var editionId = key.GetValue("EditionID")?.ToString();
                if (!string.IsNullOrEmpty(productName))
                    info.VersionName = productName;
                if (!string.IsNullOrEmpty(editionId))
                    info.Edition = editionId;
            }
        }
        catch { }

        return info;
    }
}