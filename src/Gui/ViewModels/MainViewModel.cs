using WindowsOptimizer.Core.Profiles;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WindowsOptimizer.Core.Models;
using WindowsOptimizer.Core.Services;
using WindowsOptimizer.Gui.Models;

namespace WindowsOptimizer.Gui.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private ProfileTemplate _selectedProfile;
    private string _progressText = "Готов";
    private string _pinInput = "";

    public ObservableCollection<ProfileTemplate> Profiles { get; } = new();
    public ObservableCollection<ProfileTemplate> UserProfiles { get; } = new();

    public ProfileTemplate SelectedProfile
    {
        get => _selectedProfile;
        set { _selectedProfile = value; OnPropertyChanged(); }
    }

    public string ProgressText
    {
        get => _progressText;
        set { _progressText = value; OnPropertyChanged(); }
    }

    public string PinInput
    {
        get => _pinInput;
        set { _pinInput = value; OnPropertyChanged(); }
    }

    public MainViewModel()
    {
        var logger = new VersionedFileLogger("Logs");
        _engine = new OptimizerEngine(logger, new BackupService(logger), new RegistryTweaksService(logger, WindowsInfoDetector.GetInfo()), new ServicesManager(logger), new StartupManager(logger), new VisualEffectsService(logger), new PowerPlanService(logger), new CleanupService(logger), new SafetyService());
        _safety = new SafetyService();
        _safety.OnProgress += msg => ProgressText = msg;
        foreach (var p in PresetProfiles.BuiltIn)
            Profiles.Add(p);
        SelectedProfile = Profiles[1];
    }

    private readonly OptimizerEngine _engine;
    private readonly SafetyService _safety;

    public void CreateProfile()
    {
        var newProfile = new ProfileTemplate { Name = "Custom " + (UserProfiles.Count + 1) };
        UserProfiles.Add(newProfile);
        Profiles.Add(newProfile);
        SelectedProfile = newProfile;
        MessageBox.Show("Новый профиль создан. Настройте параметры.");
    }

    public void SaveProfile()
    {
        if (SelectedProfile == null) return;
        ProfileLoader.Save($"Profiles/{SelectedProfile.Name}.json", new Profile { Name = SelectedProfile.Name, Mode = SelectedProfile.Mode });
        MessageBox.Show($"Профиль '{SelectedProfile.Name}' сохранен.");
    }

    public void DeleteProfile()
    {
        if (SelectedProfile == null) return;
        var result = MessageBox.Show($"Удалить профиль '{SelectedProfile.Name}'?", "Подтверждение", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            Profiles.Remove(SelectedProfile);
            SelectedProfile = Profiles[0];
        }
    }

    public void RunOptimization()
    {
        if (SelectedProfile == null) return;
        if (!_safety.ValidatePin(PinInput))
        {
            MessageBox.Show("Неверный PIN. Введите 111111 для подтверждения.");
            ProgressText = "PIN не подтвержден";
            return;
        }

        ProgressText = "Запуск оптимизации...";
        var results = _engine.Run(SelectedProfile.Mode, PinInput);
        var successCount = results.Count(r => r.Success);
        ProgressText = $"Завершено: {successCount}/{results.Count()} операций";
        MessageBox.Show($"Оптимизация профиля '{SelectedProfile.Name}' завершена. Успешно: {successCount}/{results.Count()}");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}