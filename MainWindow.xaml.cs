using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AmongUsLauncher.Helpers;
using AmongUsLauncher.Models;
using AmongUsLauncher.Services;

namespace AmongUsLauncher;

public partial class MainWindow : Window
{
    private readonly AppConfig _config;
    private readonly UpdateService _updateService;
    private readonly DownloadService _downloadService;
    private readonly ExtractService _extractService;
    private readonly LaunchService _launchService;
    private bool _isDownloading;

    public ObservableCollection<LogEntry> LogEntries { get; } = new();

    public MainWindow()
    {
        InitializeComponent();

        _config = ConfigHelper.Load();
        _updateService = new UpdateService(_config.RawUrl);
        _downloadService = new DownloadService();
        _extractService = new ExtractService();
        _launchService = new LaunchService();

        LogList.ItemsSource = LogEntries;

        if (!string.IsNullOrEmpty(_config.ExeName))
        {
            PlayButton.IsEnabled = true;
            StatusText.Text = $"Ready to play — {_config.ExeName}";
        }
    }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        if (_isDownloading) return;

        _isDownloading = true;
        DownloadButton.IsEnabled = false;
        PlayButton.IsEnabled = false;
        ProgressBar.Visibility = Visibility.Visible;
        ProgressBar.Value = 0;

        try
        {
            Log("Fetching latest version info...", LogType.Info);
            var (downloadUrl, exeName) = await _updateService.FetchLatestAsync();

            if (string.IsNullOrEmpty(downloadUrl))
            {
                Log("No download link found in the response", LogType.Error);
                return;
            }

            Log($"Download link: {downloadUrl}", LogType.Info);

            if (!string.IsNullOrEmpty(exeName))
                Log($"Expected executable: {exeName}", LogType.Info);

            var installDir = _config.ResolvedInstallPath;
            Directory.CreateDirectory(installDir);

            var ext = Path.GetExtension(downloadUrl)?.Split('?')[0] ?? ".zip";
            if (string.IsNullOrEmpty(ext)) ext = ".zip";
            var archivePath = Path.Combine(installDir, $"amongus_dl{ext}");

            Log("Downloading...", LogType.Progress);
            var progress = new Progress<double>(p =>
                Dispatcher.Invoke(() => ProgressBar.Value = p * 100));

            await _downloadService.DownloadAsync(downloadUrl, archivePath, progress);
            Log("Download complete", LogType.Success);

            Log("Extracting archive...", LogType.Progress);
            await _extractService.ExtractAsync(archivePath, installDir);
            Log("Extraction complete", LogType.Success);

            try { File.Delete(archivePath); } catch { }

            if (!string.IsNullOrEmpty(exeName))
            {
                var found = FindExeRecursive(installDir, exeName);
                if (found != null)
                {
                    _config.ExeName = exeName;
                    ConfigHelper.Save(_config);
                    PlayButton.IsEnabled = true;
                    StatusText.Text = $"Ready to play — {exeName}";
                    StatusText.Foreground = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(76, 175, 80));
                    Log($"'{exeName}' is ready to launch", LogType.Success);
                }
                else
                {
                    Log($"'{exeName}' not found in extracted files — check the folder", LogType.Error);
                }
            }
        }
        catch (Exception ex)
        {
            Log($"Error: {ex.Message}", LogType.Error);
        }
        finally
        {
            _isDownloading = false;
            DownloadButton.IsEnabled = true;
        }
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_config.ExeName))
        {
            Log("No game installed. Click Download first.", LogType.Error);
            return;
        }

        var installDir = _config.ResolvedInstallPath;
        var exePath = FindExeRecursive(installDir, _config.ExeName);

        if (exePath == null)
        {
            Log($"'{_config.ExeName}' not found in {installDir}", LogType.Error);
            return;
        }

        try
        {
            Log($"Launching '{exePath}'...", LogType.Progress);
            _launchService.Launch(exePath);
            Log("Game launched!", LogType.Success);
        }
        catch (Exception ex)
        {
            Log($"Failed to launch: {ex.Message}", LogType.Error);
        }
    }

    private string? FindExeRecursive(string directory, string exeName)
    {
        try
        {
            return Directory.GetFiles(directory, exeName, SearchOption.AllDirectories)
                            .FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    private void Log(string message, LogType type = LogType.Info)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Message = message,
            Type = type
        };

        Dispatcher.Invoke(() =>
        {
            LogEntries.Add(entry);
            if (LogScrollViewer.ViewportHeight + LogScrollViewer.VerticalOffset >=
                LogScrollViewer.ExtentHeight - 20)
            {
                LogScrollViewer.ScrollToBottom();
            }
        });
    }
}
