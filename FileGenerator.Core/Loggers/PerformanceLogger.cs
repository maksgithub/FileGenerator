using System;
using System.Diagnostics;

namespace FileGenerator.Core.Loggers
{
    internal class PerformanceLogger : IPerformanceLogger
    {
        private readonly string _message;
        private readonly Stopwatch _stopwatch;

        public PerformanceLogger(string message, bool showStartMessage = true)
        {
            _message = message;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            if (showStartMessage)
            {
                Console.WriteLine($"Start {_message}");
            }
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Console.WriteLine($"Finish {_message} : take {_stopwatch.ElapsedMilliseconds} ms");
        }
    }
}