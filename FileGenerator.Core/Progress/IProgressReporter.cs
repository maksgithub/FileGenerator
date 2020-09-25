using System;

namespace FileGenerator.Core.Progress
{
    public interface IProgressReporter : IDisposable
    {
        void Report(long processed);
    }
}