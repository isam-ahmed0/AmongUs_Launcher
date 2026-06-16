using System;
using System.Windows.Media;

namespace AmongUsLauncher.Models;

public enum LogType
{
    Info,
    Success,
    Error,
    Progress
}

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = "";
    public LogType Type { get; set; }

    public string DisplayText => $"[{Timestamp:HH:mm:ss}] {Message}";

    private static readonly Brush _infoBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(200, 200, 200));
    private static readonly Brush _successBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80));
    private static readonly Brush _errorBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(244, 67, 54));
    private static readonly Brush _progressBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 188, 212));

    public Brush Color => Type switch
    {
        LogType.Success => _successBrush,
        LogType.Error => _errorBrush,
        LogType.Progress => _progressBrush,
        _ => _infoBrush
    };
}
