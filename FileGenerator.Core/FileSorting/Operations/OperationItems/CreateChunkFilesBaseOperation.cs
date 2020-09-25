using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FileGenerator.Core.Common;
using FileGenerator.Core.Common.Comparers;
using FileGenerator.Core.Loggers;

namespace FileGenerator.Core.FileSorting.Operations.OperationItems
{
    public class CreateChunkFilesOperation : BaseOperation
    {
        private readonly ChannelReader<string> _channelReader;
        private readonly ChannelWriter<FileInfo> _channelWriter;
        private readonly SortingPathsHelper _pathsHelper;
        private readonly StringDataItemComparer _comparer = new StringDataItemComparer();
        private readonly ISettings _settings;
        public override OperationType OperationType => OperationType.CreateChunks;

        internal CreateChunkFilesOperation(ChannelReader<string> channelReader,
            ChannelWriter<FileInfo> channelWriter,
            SortingPathsHelper pathsHelper,
            ISettings settings)
        {
            _channelReader = channelReader;
            _channelWriter = channelWriter;
            _pathsHelper = pathsHelper;
            _settings = settings;
        }

        protected override async Task ExecuteOperationAsync()
        {
            await Task.Yield();
            var items = new List<string>(_settings.ChunkLinesCount);
            var size = 0;
            while (await _channelReader.WaitToReadAsync())
            {
                while (size <= _settings.ChunkSize && items.Count < _settings.ChunkLinesCount)
                {
                    try
                    {
                        var item = await _channelReader.ReadAsync();
                        size += item.Length;
                        items.Add(item);
                    }
                    catch (ChannelClosedException)
                    {
                        break;
                    }
                }

                items.Sort(_comparer);
                var file = WriteToFile(items);
                _channelWriter.TryWrite(file);
                items = new List<string>(_settings.ChunkLinesCount);
                size = 0;
            }
        }

        private FileInfo WriteToFile(List<string> items)
        {
            var filePath = _pathsHelper.GetRandomChunkPath();
            FileSystemHelper.CreateEmptyFile(filePath, true);
            using var writer = new StreamWriter(filePath);
            foreach (var dataItem in items)
            {
                writer.WriteLine(dataItem);
            }
            return new FileInfo(filePath);
        }
    }
}