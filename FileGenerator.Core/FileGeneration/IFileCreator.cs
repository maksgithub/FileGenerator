using System;
using System.IO;
using System.Threading.Tasks;
using FileGenerator.Core.Progress;

namespace FileGenerator.Core.FileGeneration
{
    public interface IFileCreator
    {
        Task<FileInfo> GenerateFileAsync(string path, long fileSize,
            IProgressReporter progressReporter = null);
    }
}