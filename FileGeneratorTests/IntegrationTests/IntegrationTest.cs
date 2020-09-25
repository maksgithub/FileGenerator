using System;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using FileGenerator.Core;
using FileGenerator.Core.Common;
using FileGenerator.Core.FileSorting;
using FileGeneratorTests.Common;
using Moq;
using NUnit.Framework;

namespace FileGeneratorTests.IntegrationTests
{
    [TestFixture]
    public class IntegrationTest
    {
        [Test]
        public async Task SortFileIntegrationTest()
        {
            var inputFilePath = await TestsHelper.CreateFile("inputSortingFile.txt", DataStorage.InputData1);
            var fileSorter = new FileSorter();
            var outputFilePath = await fileSorter.SortFileAsync(inputFilePath);
            var actual = await File.ReadAllTextAsync(outputFilePath);

            Assert.AreEqual(DataStorage.ExpectedData1, actual);
        }
    }
}