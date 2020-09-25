using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileGenerator.Core.Common
{
    class Settings : ISettings
    {
        public Settings(FileInfo sourceFile)
        {
            ChunkSize = sourceFile.Length / MaximumOperationsCount - 1;
        }

        public int InputFileReadBufferSize => 8096;
        public int FileWriteBufferSize => 8096;
        public int MaxSymbolsInLine => 400;
        public byte DotByte { get; } = Encoding.ASCII.GetBytes(".").Single();
        public byte[] NewLineBytes { get; } = Encoding.ASCII.GetBytes(Environment.NewLine);
        public int MaximumOperationsCount { get; } = Environment.ProcessorCount;
        public long ChunkSize { get; }
        public int ChunkLinesCount { get; } = 700_000;
    }
}