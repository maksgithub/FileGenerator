using System.Threading.Tasks;

namespace FileGenerator.Core.FileGeneration
{
    public interface IFileCreator
    {
        Task GenerateFileAsync(string path, int linesCount, int duplicatesCount);
    }
}