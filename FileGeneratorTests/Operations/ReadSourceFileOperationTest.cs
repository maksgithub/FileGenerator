using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using FileGenerator.Core.Common;
using FileGenerator.Core.FileSorting.Operations.OperationItems;
using FileGeneratorTests.Common;
using NUnit.Framework;

namespace FileGeneratorTests.Operations
{
    [TestFixture]
    public class ReadSourceFileOperationTest
    {
        [Test]
        public async Task ReadSourceFileOperation()
        {
            var inputFilePath = await TestsHelper.CreateFile($"{Guid.NewGuid().ToString()}.txt",
                DataStorage.ExpectedData1);
            var channel = Channel.CreateUnbounded<string>();
            var fileOperation = new ReadSourceFileOperation(inputFilePath, TestsHelper.GetSettings(), 
                channel.Writer);
            fileOperation.ExecuteAsync();
            var items = await channel.Reader.ReadAllAsync().ToListAsync();
            var data = items.Select(x => x).ToList();
            CollectionAssert.AreEqual(new List<string>()
            {
                "1. Apple",
                "415. Apple",
                "2. Banana is yellow",
                "2. Banana is yellow",
                "32. Cherry is the best",
                "30432. Something something something",
                "0. Windows",
            }, data);
        }

        [Test]
        public async Task ReadSourceFileOperation_2()
        {
            var fileName = $"{Guid.NewGuid().ToString()}.txt";
            var inputFilePath = await TestsHelper.CreateFile(fileName, "1. Apple\r\n");
            var channel = Channel.CreateUnbounded<string>();
            var fileOperation = new ReadSourceFileOperation(inputFilePath, TestsHelper.GetSettings(), 
                channel.Writer);
            fileOperation.ExecuteAsync();
            var items = await channel.Reader.ReadAllAsync().ToListAsync();
            var data = items.Select(x => x).ToList();
            CollectionAssert.AreEqual(new List<string>()
            {
                "1. Apple"
            }, data);
        }

        [Test]
        public async Task ReadSourceFileOperation_3()
        {
            var fileName = $"{Guid.NewGuid().ToString()}.txt";
            var inputFilePath = await TestsHelper.CreateFile(fileName, "1. Apple\r\n415. Apple\r\n");
            var channel = Channel.CreateUnbounded<string>();
            var fileOperation = new ReadSourceFileOperation(inputFilePath, TestsHelper.GetSettings(), 
                channel.Writer);
            fileOperation.ExecuteAsync();
            var items = await channel.Reader.ReadAllAsync().ToListAsync();
            var data = items.Select(x => x).ToList();
            CollectionAssert.AreEqual(new List<string>()
            {
                "1. Apple",
                "415. Apple"
            }, data);
        }
    }
}