using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using FileGenerator.Core.Common;
using FileGenerator.Core.Loggers;

namespace FileGenerator.Core.FileSorting.Operations.OperationItems
{
    public class MergeChunksOperation : BaseOperation
    {
        private readonly Channel<FileInfo> _chunksChannel;
        private readonly FileInfo _file1;
        private readonly FileInfo _file2;
        private readonly SortingPathsHelper _pathsHelper;
        public override OperationType OperationType => OperationType.MergeChunks;

        public MergeChunksOperation(Channel<FileInfo> chunksChannel,
            FileInfo file1, FileInfo file2, SortingPathsHelper pathsHelper)
        {
            _chunksChannel = chunksChannel;
            _file1 = file1;
            _file2 = file2;
            _pathsHelper = pathsHelper;
        }

        protected override async Task ExecuteOperationAsync()
        {
            await Task.Yield();
            var destination = _pathsHelper.GetRandomChunkPath();
            FileSystemHelper.MergeFiles(_file1.FullName, _file2.FullName, destination);
            _file1.Delete();
            _file2.Delete();
            _chunksChannel.Writer.TryWrite(new FileInfo(destination));
            TryFinishSorting();
        }

        private void TryFinishSorting()
        {
            if (IsFinished(out var file))
            {
                _chunksChannel.Writer.Complete();
                if (File.Exists(_pathsHelper.SortedFilePath))
                {
                    File.Delete(_pathsHelper.SortedFilePath);
                }
                file.MoveTo(_pathsHelper.SortedFilePath);
                _pathsHelper.ChunksDirectory.Delete();
                FileSystemHelper.RemoveNewLineFromEnd(file);
            }
        }

        protected override IPerformanceLogger GetLogger()
        {
            var size1 = BytesHelper.GetHumanStringFromBytes(_file1.Length);
            var size2 = BytesHelper.GetHumanStringFromBytes(_file2.Length);
            return FileGeneratorFactory.GetLogger($"{OperationType} - {size1} + {size2}", false);
        }

        private bool IsFinished(out FileInfo file)
        {
            file = null;
            try
            {
                if (_pathsHelper.ChunksDirectory.Exists)
                {
                    return _pathsHelper.ChunksDirectory.EnumerateFiles().HasSingle(out file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
    }
}