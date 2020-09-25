using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FileGenerator.Core.Common;
using FileGenerator.Core.Progress;

namespace FileGenerator.Core.FileGeneration
{
    internal class FileCreator : IFileCreator
    {
        private const int MaximumBufferCount = 1000;
        private const int GenerateOperationsCount = 3;

        public async Task<FileInfo> GenerateFileAsync(string path, long fileSize, int duplicatesCount,
            IProgressReporter progressReporter)
        {
            await FileSystemHelper.CreateEmptyFileAsync(path, true);
            var writeItemsTask = WriteItemsToFileAsync(path, progressReporter, fileSize);
            await Task.WhenAll(writeItemsTask);
            return new FileInfo(path);
        }

        private async Task WriteItemsToFileAsync(string filePath, IProgressReporter progressReporter, long fileSize)
        {
            var token = new CancellationTokenSource();
            var stack = new ConcurrentStack<char[]>();

            var tasks = new List<Task>();
            for (int i = 0; i < GenerateOperationsCount; i++)
            {
                tasks.Add(GenerateItemsAsync(stack, token.Token));
            }

            await using var fileStream = new StreamWriter(filePath);
            while (fileStream.BaseStream.Position < fileSize)
            {
                if(stack.TryPop(out var dataItem))
                {
                    fileStream.WriteLine(dataItem);
                    progressReporter?.Report(fileStream.BaseStream.Position);
                }
            }
            token.Cancel();
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private Task GenerateItemsAsync(ConcurrentStack<char[]> concurrentStack, CancellationToken token)
        {
            return Task.Run(() =>
            {
                var generator = new DataItemGenerator();
                while (!token.IsCancellationRequested)
                {
                    if (concurrentStack.Count < MaximumBufferCount)
                    {
                        concurrentStack.Push(generator.GenerateItem());
                    }
                }
            }, token);
        }
    }
}