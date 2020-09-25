using System.Threading.Tasks;

namespace FileGenerator.Core.FileSorting
{
    public interface IFileSorter
    {
        Task<string> SortFileAsync(string filePath);
    }
}