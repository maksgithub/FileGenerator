using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileGenerator.Core.Common;
using FileGeneratorTests.IntegrationTests;
using Moq;

namespace FileGeneratorTests.Common
{
    public static class TestsHelper
    {
        public static async Task<string> CreateFile(string fileName, string data)
        {
            var executionDirectory = Path.GetDirectoryName(typeof(IntegrationTest).Assembly.Location);
            var dataPath = Path.Combine(executionDirectory, "IntegrationTest");
            FileSystemHelper.DeleteDirectory(dataPath);
            var inputFilePath = Path.Combine(dataPath, fileName);
            await FileSystemHelper.CreateEmptyFileAsync(inputFilePath, true);
            await File.WriteAllTextAsync(inputFilePath, data);
            return inputFilePath;
        }

        public static ISettings GetSettings(int maxSymbolsInLine = 200)
        {
            var mock = new Mock<ISettings>();
            mock.SetupGet(x => x.InputFileReadBufferSize).Returns(10);
            mock.SetupGet(x => x.MaxSymbolsInLine).Returns(maxSymbolsInLine);
            mock.SetupGet(x => x.NewLineBytes).Returns(Encoding.ASCII.GetBytes(Environment.NewLine));
            mock.SetupGet(x => x.DotByte).Returns(Encoding.ASCII.GetBytes(".").Single());
            mock.SetupGet(x => x.MaximumOperationsCount).Returns(2);
            mock.SetupGet(x => x.ChunkSize).Returns(4);
            var settings = mock.Object;
            return settings;
        }
    }
}