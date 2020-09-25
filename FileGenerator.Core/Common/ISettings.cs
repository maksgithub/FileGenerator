using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FileGeneratorTests")]
namespace FileGenerator.Core.Common
{
    public interface ISettings
    {
        int InputFileReadBufferSize { get; }
        int FileWriteBufferSize { get; }
        int MaxSymbolsInLine { get; }
        byte DotByte { get; }
        byte[] NewLineBytes { get; }
        int MaximumOperationsCount { get; }
        long ChunkSize { get; }
        int ChunkLinesCount { get; }
    }
}