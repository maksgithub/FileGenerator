using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FileHelpers;

namespace FileGenerator.Core
{
    public class FileCreator : IFileCreator
    {
        public FileCreator()
        {
        }

        [Benchmark]
        public async Task GenerateFileAsync(string path, int linesCount, int duplicatesCount)
        {
            for (int i = 0; i < linesCount; i++)
            {
                var item = $"{i}{Environment.NewLine}";
                var engine = new FileHelperEngine<Orders>();
                engine.AppendToFile(path, new Orders() { CustomerID = item });
            }

            await Task.CompletedTask;
        }

        private async Task WriteLineAsync(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }


            await using Stream stream = new FileStream(path, FileMode.CreateNew);
           
        }
    }
}