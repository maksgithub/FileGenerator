using System;
using System.Collections.Generic;
using System.Text;
using FileGenerator.Core.FileGeneration;
using FileGenerator.Core.Helpers;

namespace FileGenerator.Core
{
    public static class FileGeneratorFactory
    {
        private static readonly IFileSystemHelper _fileSystemHelper = new FileSystemHelper();

        public static IFileCreator GetFileCreator()
        {
            return new FileCreator(_fileSystemHelper);
        }
    }
}
