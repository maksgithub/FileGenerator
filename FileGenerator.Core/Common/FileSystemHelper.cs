using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileGenerator.Core.Common.Comparers;

namespace FileGenerator.Core.Common
{
    public static class FileSystemHelper
    {
        private static readonly byte[] NewLine = Encoding.ASCII.GetBytes(Environment.NewLine);
        static readonly StringDataItemComparer Comparer = new StringDataItemComparer();

        public static async Task CreateEmptyFileAsync(string filePath, bool overwrite)
        {
            if (overwrite && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath);
            await using var file = File.Create(filePath);
        }

        public static void CreateEmptyFile(string filePath, bool overwrite)
        {
            if (overwrite && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath);
            using var file = File.Create(filePath);
        }

        public static void DeleteDirectory(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
            }
        }

        public static void RemoveNewLineFromEnd(Stream fileStream)
        {
            fileStream.Seek(-1, SeekOrigin.End);
            var buffer = new byte[1];
            while (fileStream.Read(buffer) > 0)
            {
                if (buffer[0] == NewLine[0] || buffer[0] == NewLine[1])
                {
                    fileStream.SetLength(fileStream.Length - 1);
                    if (fileStream.Length > 0)
                    {
                        fileStream.Seek(-1, SeekOrigin.End);
                    }
                }
                else
                {
                    break;
                }
            }

        }

        public static void RemoveNewLineFromEnd(FileInfo fileInfo)
        {
            using var stream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
            RemoveNewLineFromEnd(stream);
        }

        public static FileInfo CreateFile(string path)
        {
            FileInfo file = null;
            try
            {
                file = new FileInfo(path);
            }
            catch (Exception)
            {
                // ignored
            }

            return file;
        }

        public static void MergeFiles(string source1, string source2, string destination)
        {
            using var reader1 = new StreamReader(source1);
            using var reader2 = new StreamReader(source2);

            using var writer = new StreamWriter(destination);
            using var iterator1 = ToIterator(reader1);
            using var iterator2 = ToIterator(reader2);
            var iterator1StillAvailable = iterator1.MoveNext();
            var iterator2StillAvailable = iterator2.MoveNext();

            while (iterator1StillAvailable && iterator2StillAvailable)
            {
                if (Comparer.Compare(iterator1.Current, iterator2.Current) <= 0)
                {
                    writer.WriteLine(iterator1.Current);
                    iterator1StillAvailable = iterator1.MoveNext();
                }
                else
                {
                    writer.WriteLine(iterator2.Current);
                    iterator2StillAvailable = iterator2.MoveNext();
                }
            }

            //check which iterator can still provide values
            var iteratorRemaining = iterator1StillAvailable
                ? iterator1
                : iterator2StillAvailable ? iterator2 : null;

            if (null != iteratorRemaining)
            {
                do
                {
                    writer.WriteLine(iteratorRemaining.Current);
                } while (iteratorRemaining.MoveNext());
            }
        }

        private static IEnumerator<string> ToIterator(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}