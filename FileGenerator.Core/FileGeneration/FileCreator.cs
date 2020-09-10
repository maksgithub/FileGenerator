using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FileGenerator.Core.Helpers;

namespace FileGenerator.Core.FileGeneration
{
    internal class FileCreator : IFileCreator
    {
        private readonly IFileSystemHelper _fileSystemHelper;

        public FileCreator(IFileSystemHelper fileSystemHelper)
        {
            _fileSystemHelper = fileSystemHelper;
        }

        [Benchmark]
        public async Task GenerateFileAsync()
        {
            await GenerateFileAsync(@"C:\Users\Max\Desktop\New folder (3)\File.txt", 1000, 0);
        }

        public async Task GenerateFileAsync(string path, int linesCount, int duplicatesCount)
        {
            await _fileSystemHelper.CreateEmptyFileAsync(path, true);

            //for (int i = 0; i < linesCount; i++)
            //{
            //    var item = $"{i}{Environment.NewLine}";
            //    var engine = new FileHelperEngine<Orders>();
            //    engine.AppendToFile(path, new Orders() { CustomerID = item });
            //}

            await WriteLineAsync(path);
        }

        private async Task WriteLineAsync(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }


            await using Stream stream = new FileStream(path, FileMode.CreateNew);
            await using Stream stream2 = new FileStream(path, FileMode.Open);
        }
    }
}