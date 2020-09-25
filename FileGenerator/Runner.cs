using System.Threading;
using System.Threading.Tasks;
using FileGenerator.Demo;

namespace FileGenerator
{
    class Runner
    {
        static async Task Main(string[] args)
        {
            var program = new DemoProgram();
            await program.Run(CancellationToken.None);
        }
    }
}
