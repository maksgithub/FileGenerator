using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using FileGenerator.Core.Common;
using FileGenerator.Core.FileSorting.Operations.OperationItems;

namespace FileGenerator.Core.FileSorting.Operations
{
    public class OperationsManager
    {
        private readonly int _operationsCount;
        private readonly Channel<IOperation> _operationsChannel;
        private readonly OperationsFactory _operationsFactory;

        public OperationsManager(ISettings settings, SortingPathsHelper pathsHelper)
        {
            _operationsCount = settings.MaximumOperationsCount;
            _operationsChannel = Channel.CreateBounded<IOperation>(_operationsCount);
            _operationsFactory = new OperationsFactory(settings, pathsHelper);
        }

        public async Task StartManageOperationsAsync()
        {
            var reader = _operationsChannel.Reader;
            var writer = _operationsChannel.Writer;

            var createTask = StartCreateOperationsAsync(writer);
            var executeTask = StartExecuteOperationsAsync(reader);
            await Task.WhenAll(createTask, executeTask);
        }

        private async Task StartExecuteOperationsAsync(ChannelReader<IOperation> reader)
        {
            var tasks = new List<Task>(_operationsCount);
            while (await reader.WaitToReadAsync().ConfigureAwait(false))
            {
                var operation = await reader.ReadAsync().ConfigureAwait(false);
                if (operation != null)
                {
                    tasks.Add(operation.ExecuteAsync());
                }

                if (tasks.Count == _operationsCount)
                {
                    var task = await Task.WhenAny(tasks).ConfigureAwait(false);
                    tasks.Remove(task);
                }
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private Task StartCreateOperationsAsync(ChannelWriter<IOperation> writer)
        {
            return Task.Run(async () =>
            {
                while (await writer.WaitToWriteAsync().ConfigureAwait(false))
                {
                    var operation = await _operationsFactory.GetOperationAsync().ConfigureAwait(false);
                    if (operation == null)
                    {
                        break;
                    }

                    await writer.WriteAsync(operation).ConfigureAwait(false);
                }
                writer.Complete();
            });
        }
    }
}