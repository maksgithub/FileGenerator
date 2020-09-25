using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using FileGenerator.Core.Common;
using FileGenerator.Core.FileSorting.Operations.OperationItems;

namespace FileGenerator.Core.FileSorting.Operations
{
    public class OperationsFactory
    {
        private int _operationCounter;
        private readonly double _operationsCount;
        private readonly Channel<string> _sourceFileChannel;
        private readonly Channel<FileInfo> _chunksChannel;
        private readonly ISettings _settings;
        private readonly SortingPathsHelper _pathsHelper;

        public OperationsFactory(ISettings settings, SortingPathsHelper pathsHelper)
        {
            _sourceFileChannel = Channel.CreateUnbounded<string>();
            _chunksChannel = Channel.CreateUnbounded<FileInfo>();
            _settings = settings;
            _pathsHelper = pathsHelper;
            _operationsCount = _settings.MaximumOperationsCount;
        }

        public async Task<IOperation> GetOperationAsync()
        {
            IOperation operation = null;
           
            if (_operationCounter == 0)
            {
                operation = new ReadSourceFileOperation(_pathsHelper.SourceFile.FullName, 
                    _settings, _sourceFileChannel.Writer);
            }
            else if (_operationCounter <= _operationsCount)
            {
                operation = new CreateChunkFilesOperation(_sourceFileChannel.Reader,
                    _chunksChannel.Writer, _pathsHelper, _settings);

            }
            else if (await _chunksChannel.Reader.WaitToReadAsync().ConfigureAwait(false))
            {
                try
                {
                    var file1 = await _chunksChannel.Reader.ReadAsync().ConfigureAwait(false);
                    var file2 = await _chunksChannel.Reader.ReadAsync().ConfigureAwait(false);
                    operation = new MergeChunksOperation(_chunksChannel, file1, file2, 
                        _pathsHelper);
                }
                catch (ChannelClosedException) { }
            }

            _operationCounter++;
            return operation;
        }
    }
}