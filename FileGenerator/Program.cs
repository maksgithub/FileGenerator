using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using FileGenerator.Core;
using FileGenerator.Core.FileGeneration;

namespace FileGenerator
{
    class Program
    {
        private static readonly IFileCreator FileCreator = FileGeneratorFactory.GetFileCreator();
        private const int L = 20000000;
        private const string Path = @"C:\Users\Max\Desktop\New folder (3)\File2.txt";
        //private static readonly FileStream CommonResource = new FileStream(Path, FileMode.CreateNew);
        //private static readonly MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(Path);

        private static async Task Main()
        {
            await FileCreator.GenerateFileAsync(Path, 10000, 0);
            var s = new Stopwatch();
            s.Start();
            await Do(new ProgressReporter(L));
            s.Stop();

            Console.WriteLine($"Total = {s.ElapsedMilliseconds}");
            Console.ReadKey();
        }

        private static async Task Do(ProgressReporter progressReporter)
        {
            var ch = Channel.CreateBounded<byte[]>(L);

            var consumer = Task.Run(async () =>
            {
                using MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(Path, FileMode.CreateNew);
                await using var a = mmf.CreateViewStream();
                using var progress = new ProgressBar(L);
                while (await ch.Reader.WaitToReadAsync().ConfigureAwait(false))
                {
                    var readAsync = await ch.Reader.ReadAsync().ConfigureAwait(false);
                    a.Write(readAsync);
                    progress.Report();
                }
            });

            var producer = Task.Run(async () =>
            {
                var rnd = new Random();
                var с = 8;
                for (int i = 0; i < L; i += с)
                {
                    await ch.Writer.WriteAsync(GetLines(с, i));
                }

                ch.Writer.Complete();
            });

            await Task.WhenAll(producer, consumer);
        }

        private static byte[] GetLines(int count, int i)
        {
            var sb = new StringBuilder();
            for (int j = 0; j < count; j++)
            {
                sb.Append((i - j).ToString());
                sb.AppendLine(
                    " Messagerrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr ");
            }
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
    }

    internal class ProgressReporter
    {
        private readonly int _max;
        private int _c;

        public ProgressReporter(in int max)
        {
            _max = max;
        }

        public void Report()
        {
            _c++;
            var t = _c / _max;
            Console.Clear();
            Console.WriteLine(t);
        }
    }
}
