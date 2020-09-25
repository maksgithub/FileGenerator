using System;
using System.Threading;
using System.Threading.Tasks;
using FileGenerator.Core.Progress;

namespace FileGenerator
{
    internal class ProgressReporter : IProgressReporter
    {
        private readonly long _totalSize;
        private long _processed;
        private readonly Timer _timer;

        public ProgressReporter(in long totalSize)
        {
            _totalSize = totalSize;
            _timer = new Timer(TimerHandler);
            _timer.Change(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(200));
        }

        private void TimerHandler(object? state)
        {
            var totalSize = (double)_processed / _totalSize * 100;
            //Console.SetCursorPosition(20, 1);
            Console.Write("\r{0:0.0000} %   ", totalSize);
        }

        public void Report(long processed = default)
        {
            _processed = processed;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}