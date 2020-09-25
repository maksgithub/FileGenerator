using System;
using System.Threading.Tasks;
using FileGenerator.Core.Loggers;

namespace FileGenerator.Core.FileSorting.Operations.OperationItems
{
    public abstract class BaseOperation : IOperation
    {
        public abstract OperationType OperationType { get; }

        public virtual async Task ExecuteAsync()
        {
            using var logger = GetLogger();
            try
            {
                await ExecuteOperationAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected virtual IPerformanceLogger GetLogger()
        {
            return  FileGeneratorFactory.GetLogger($"{OperationType}");
        }

        protected abstract Task ExecuteOperationAsync();
    }
}