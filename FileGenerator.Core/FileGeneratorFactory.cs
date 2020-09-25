using System;
using System.Collections.Generic;
using System.Text;
using FileGenerator.Core.Common;
using FileGenerator.Core.FileGeneration;
using FileGenerator.Core.FileSorting;
using FileGenerator.Core.Loggers;

namespace FileGenerator.Core
{
    public static class FileGeneratorFactory
    {
        public static IFileCreator GetFileCreator()
        {
            return new FileCreator();
        }

        public static IFileSorter GetFileSorter()
        {
            return new FileSorter();
        }

        public static IPerformanceLogger GetLogger(string message, bool showStartMessage = true)
        {
            return new PerformanceLogger(message, showStartMessage);
        }
    }
}
