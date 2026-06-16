using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AmongUsLauncher.Services;

public class DownloadService
{
    public async Task DownloadAsync(string url, string destPath, IProgress<double> progress, CancellationToken ct = default)
    {
        using var client = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(30)
        };

        using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();

        var totalBytes = response.Content.Headers.ContentLength ?? -1;
        await using var contentStream = await response.Content.ReadAsStreamAsync(ct);
        await using var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

        var buffer = new byte[81920];
        long bytesRead = 0;
        int read;

        while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length, ct)) > 0)
        {
            await fileStream.WriteAsync(buffer, 0, read, ct);
            bytesRead += read;
            if (totalBytes > 0)
                progress.Report((double)bytesRead / totalBytes);
        }
    }
}
