using System.Text.Json;
using WindowsOptimizer.Core.Models;

namespace WindowsOptimizer.Core.Profiles;

public class Profile
{
    public string Name { get; set; } = "Custom";
    public OperationMode Mode { get; set; } = OperationMode.Balanced;
    public bool RegistryTweaks { get; set; } = true;
    public bool Services { get; set; } = true;
    public bool VisualEffects { get; set; } = true;
}

public static class ProfileLoader
{
    public static Profile Load(string path)
    {
        if (!File.Exists(path))
            return new Profile();
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Profile>(json) ?? new Profile();
    }

    public static void Save(string path, Profile profile)
    {
        var json = JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}