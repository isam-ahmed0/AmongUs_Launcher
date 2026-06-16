using System;
using System.IO;
using System.Text.Json;

namespace AmongUsLauncher.Helpers;

public class AppConfig
{
    public string RawUrl { get; set; } = "https://raw.githubusercontent.com/YOUR_USER/YOUR_REPO/main/amongus.txt";
    public string InstallPath { get; set; } = "%LOCALAPPDATA%\\AmongUsLauncher";
    public string ExeName { get; set; } = "";

    public string ResolvedInstallPath => Environment.ExpandEnvironmentVariables(InstallPath);
}

public static class ConfigHelper
{
    private static readonly string ConfigPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

    public static AppConfig Load()
    {
        if (!File.Exists(ConfigPath))
            return new AppConfig();

        var json = File.ReadAllText(ConfigPath);
        return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
    }

    public static void Save(AppConfig config)
    {
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigPath, json);
    }
}
