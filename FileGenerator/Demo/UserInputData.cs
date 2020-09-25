using System;
using System.IO;

namespace FileGenerator.Demo
{
    public class UserInputData
    {
        private const int DefaultSizeInBytes = 100 * 1024 * 1024;

        public UserInputData()
        {
            ResetDefaults();
        }

        public string FilePath { get; set; }
        public long Size { get; set; }

        internal void ResetDefaults()
        {
            FilePath = GetDefaultPath();
            Size = DefaultSizeInBytes;
        }


        private static string GetDefaultPath()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var defaultPath = Path.Combine(desktopPath, "FileGen", "File_initial.txt");
            return defaultPath;
        }
    }
}