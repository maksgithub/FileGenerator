using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using FileGenerator.Core.Common;

[assembly: InternalsVisibleTo("FileGeneratorTests")]
namespace FileGenerator.Core.FileSorting.Operations.OperationItems
{
    public class ReadSourceFileOperation : BaseOperation
    {
        private readonly string _filePath;
        private readonly ISettings _settings;
        private readonly ChannelWriter<string> _channelWriter;
        public override OperationType OperationType { get; } = OperationType.ReadSourceFile;

        internal ReadSourceFileOperation(string filePath, ISettings settings, 
            ChannelWriter<string> channelWriter)
        {
            _filePath = filePath;
            _settings = settings;
            _channelWriter = channelWriter;
        }

        protected override Task ExecuteOperationAsync()
        {
            return Task.Run(Write);
        }

        private void Write()
        {
            using var binaryReader = new StreamReader(_filePath, Encoding.UTF8, true, 4096 * 2);
            string line;
            while ((line = binaryReader.ReadLine()) != null && line.Length > 2)
            {
                _channelWriter.TryWrite(line);
            }
            _channelWriter.Complete();
        }
    }
}