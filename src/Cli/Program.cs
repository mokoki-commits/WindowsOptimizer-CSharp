using WindowsOptimizer.Core.Models;
using WindowsOptimizer.Core.Services;

Console.WriteLine($"Windows Optimizer C# - Hybrid Edition");
Console.WriteLine($"Version: {VersionInfo.ProductName} v{VersionInfo.Version} Build {VersionInfo.BuildDate}");
Console.WriteLine("=====================================");

var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
var logger = new VersionedFileLogger(logDir);
logger.Info($"Started {VersionInfo.ProductName} v{VersionInfo.Version}");

var winInfo = WindowsInfoDetector.GetInfo();
Console.WriteLine($"Detected: {winInfo.VersionName} Build {winInfo.BuildNumber}");

var mode = OperationMode.Strict;
if (args.Length > 0 && Enum.TryParse<OperationMode>(args[0], true, out var parsed))
    mode = parsed;

var pin = args.Length > 1 ? args[1] : null;

logger.Info($"Starting optimizer in mode {mode}");

var backup = new BackupService(logger);
var registry = new RegistryTweaksService(logger, winInfo);
var services = new ServicesManager(logger);
var startup = new StartupManager(logger);
var visual = new VisualEffectsService(logger);
var power = new PowerPlanService(logger);
var cleanup = new CleanupService(logger);
var safety = new SafetyService();

var engine = new OptimizerEngine(logger, backup, registry, services, startup, visual, power, cleanup, safety);

var results = engine.Run(mode, pin);

Console.WriteLine("\nResults:");
foreach (var r in results)
{
    Console.WriteLine($"[{r.Category}] {r.Name}: {(r.Success ? "OK" : "FAIL")} - {r.Message}");
    logger.Log(r);
}

Console.WriteLine($"\nLog file: {((FileLogger)logger).LogPath}");
Console.WriteLine("Done.");