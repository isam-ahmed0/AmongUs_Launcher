using System.Diagnostics;
using System.IO;

namespace AmongUsLauncher.Services;

public class LaunchService
{
    public void Launch(string exePath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = exePath,
            WorkingDirectory = Path.GetDirectoryName(exePath),
            UseShellExecute = true
        };

        Process.Start(startInfo);
    }
}
