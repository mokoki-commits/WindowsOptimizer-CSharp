using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Gui.Models;

public class ProfileTemplate : INotifyPropertyChanged
{
    private string _name = "Custom";
    private OperationMode _mode = OperationMode.Balanced;
    private bool _registryTweaks = true;
    private bool _services = true;
    private bool _visualEffects = true;
    private bool _powerPlan = true;
    private bool _cleanup = true;
    private bool _startup = true;

    public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
    public OperationMode Mode { get => _mode; set { _mode = value; OnPropertyChanged(); } }
    public bool RegistryTweaks { get => _registryTweaks; set { _registryTweaks = value; OnPropertyChanged(); } }
    public bool Services { get => _services; set { _services = value; OnPropertyChanged(); } }
    public bool VisualEffects { get => _visualEffects; set { _visualEffects = value; OnPropertyChanged(); } }
    public bool PowerPlan { get => _powerPlan; set { _powerPlan = value; OnPropertyChanged(); } }
    public bool Cleanup { get => _cleanup; set { _cleanup = value; OnPropertyChanged(); } }
    public bool Startup { get => _startup; set { _startup = value; OnPropertyChanged(); } }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public static class PresetProfiles
{
    public static List<ProfileTemplate> BuiltIn => new()
    {
        new ProfileTemplate
        {
            Name = "Safe",
            Mode = OperationMode.Safe,
            RegistryTweaks = true,
            Services = false,
            VisualEffects = true,
            PowerPlan = true,
            Cleanup = true,
            Startup = false
        },
        new ProfileTemplate
        {
            Name = "Balanced",
            Mode = OperationMode.Balanced,
            RegistryTweaks = true,
            Services = true,
            VisualEffects = true,
            PowerPlan = true,
            Cleanup = true,
            Startup = true
        },
        new ProfileTemplate
        {
            Name = "Strict",
            Mode = OperationMode.Strict,
            RegistryTweaks = true,
            Services = true,
            VisualEffects = true,
            PowerPlan = true,
            Cleanup = true,
            Startup = true
        },
        new ProfileTemplate
        {
            Name = "Gaming",
            Mode = OperationMode.Balanced,
            RegistryTweaks = true,
            Services = true,
            VisualEffects = false,
            PowerPlan = true,
            Cleanup = true,
            Startup = true
        }
    };
}