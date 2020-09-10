using System.IO;
using System.Threading.Tasks;

namespace FileGenerator.Core.Helpers
{
    public class FileSystemHelper : IFileSystemHelper
    {
        public async Task CreateEmptyFileAsync(string filePath, bool overwrite)
        {
            if (overwrite && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath);
            await using var file = File.Create(filePath);
        }
    }
}