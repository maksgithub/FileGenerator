using System.Threading.Tasks;

namespace FileGenerator.Core.Helpers
{
    public interface IFileSystemHelper
    {
        Task CreateEmptyFileAsync(string filePath, bool overwrite);
    }
}