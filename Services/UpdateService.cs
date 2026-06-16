using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AmongUsLauncher.Services;

public class UpdateService
{
    private readonly string _rawUrl;

    public UpdateService(string rawUrl)
    {
        _rawUrl = rawUrl;
    }

    public async Task<(string? downloadUrl, string? exeName)> FetchLatestAsync()
    {
        using var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        var content = await client.GetStringAsync(_rawUrl);

        string? downloadUrl = null;
        string? exeName = null;

        foreach (var line in content.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith("link:", StringComparison.OrdinalIgnoreCase))
                downloadUrl = trimmed["link:".Length..].Trim();
            else if (trimmed.StartsWith("exe:", StringComparison.OrdinalIgnoreCase))
                exeName = trimmed["exe:".Length..].Trim();
        }

        return (downloadUrl, exeName);
    }
}
