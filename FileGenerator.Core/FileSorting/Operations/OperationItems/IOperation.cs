using System.Threading.Tasks;

namespace FileGenerator.Core.FileSorting.Operations.OperationItems
{
    public interface IOperation
    {
        OperationType OperationType { get; }
        Task ExecuteAsync();
    }
}