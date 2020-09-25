using System.Threading.Tasks;
using FileGenerator.Core.Common;
using FileGenerator.Core.FileSorting.Operations;

namespace FileGenerator.Core.FileSorting
{
    internal class FileSorter : IFileSorter
    {
        public async Task<string> SortFileAsync(string filePath)
        {
            var pathHelper = new SortingPathsHelper(filePath);
            FileSystemHelper.DeleteDirectory(pathHelper.ChunksDirectory.FullName);
            var settings = new Settings(pathHelper.SourceFile);
            var operationsManager = new OperationsManager(settings, pathHelper);
            await operationsManager.StartManageOperationsAsync();
            return pathHelper.SortedFilePath;
        }
    }
}