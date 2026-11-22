namespace WindowsOptimizer.Core.Services;

public interface ISafetyService
{
    bool ConfirmAction(string actionDescription, string? pinInput = null);
    bool ValidatePin(string inputPin);
    event Action<string>? OnProgress;
    event Action<string>? OnLog;
    void ReportProgress(string msg);
    void ReportLog(string msg);
}

public class SafetyService : ISafetyService
{
    private const string DefaultPin = "111111";
    
    public event Action<string>? OnProgress;
    public event Action<string>? OnLog;

    public bool ValidatePin(string inputPin) => inputPin == DefaultPin;

    public bool ConfirmAction(string actionDescription, string? pinInput = null)
    {
        OnLog?.Invoke($"[SAFETY] {actionDescription}");
        if (pinInput == null)
        {
            OnLog?.Invoke("PIN required");
            return false;
        }
        return ValidatePin(pinInput);
    }

    public void ReportProgress(string msg) => OnProgress?.Invoke(msg);
    public void ReportLog(string msg) => OnLog?.Invoke(msg);
}
