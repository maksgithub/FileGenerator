using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using FileGenerator.Core;

namespace FileGenerator
{
    class Program
    {
        private static FileCreator _ss;

        
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FileCreator>();
            Console.WriteLine(summary.Table);
            Console.ReadKey();
        }
    }
}
