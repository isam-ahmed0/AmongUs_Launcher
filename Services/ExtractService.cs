using System.IO;
using System.Threading.Tasks;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace AmongUsLauncher.Services;

public class ExtractService
{
    public async Task ExtractAsync(string archivePath, string destDir)
    {
        await Task.Run(() =>
        {
            Directory.CreateDirectory(destDir);

            using var stream = File.OpenRead(archivePath);
            using var reader = ReaderFactory.OpenReader(stream);
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                {
                    reader.WriteEntryToDirectory(destDir, new ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        });
    }
}
