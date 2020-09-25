using System;
using System.IO;
using FileGenerator.Core.Common;

namespace FileGenerator.Core.FileSorting
{
    public class SortingPathsHelper
    {
        private const string ChunksDirName = "chunks";

        public FileInfo SourceFile { get; }
        public DirectoryInfo ChunksDirectory { get; }
        public string SortedFilePath { get; }

        public SortingPathsHelper(string sourceFilePath)
        {
            SourceFile = new FileInfo(sourceFilePath);
            var chunksDirPath = SourceFile.DirectoryName.Combine(ChunksDirName);
            ChunksDirectory = new DirectoryInfo(chunksDirPath);
            var sortedFileName = $"{Path.GetFileNameWithoutExtension(SourceFile.Name)}_sorted.txt";
            SortedFilePath = SourceFile.DirectoryName.Combine(sortedFileName);
        }

        public string GetRandomChunkPath()
        {
            return ChunksDirectory.FullName.Combine(GetRandomFileName());
        }

        public static string GetRandomFileName()
        {
            return $"{Guid.NewGuid().ToString()}.txt";
        }
    }
}