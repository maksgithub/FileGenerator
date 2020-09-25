using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EasyConsole;
using FileGenerator.Core;
using FileGenerator.Core.Common;
using FileGenerator.Demo.Pages.GenerateFilePages;
using FileGenerator.Demo.Pages.SortFile;

namespace FileGenerator.Demo.Pages
{
    class MainPage : MenuPage
    {
        private UserInputData _userData;

        public MainPage(Program program, UserInputData userData) : base("Start", program)
        {
            _userData = userData;
            this.Menu.Add(new Option("Generate file", token => GenerateFileAsync(program, userData, token)));
            Menu.Add(new Option($"Sort file", token => SortFileAsync(program, userData, token)));
        }

        public override Task Display(CancellationToken cancellationToken)
        {
            _userData.ResetDefaults();
            return base.Display(cancellationToken);
        }

        private static async Task SortFileAsync(Program program, UserInputData userData, CancellationToken token)
        {
            await program.NavigateTo<ChooseSortFilePage>(token);
            var path = userData.FilePath;
            if (CanStartSorting(path))
            {
                await SortFileAsync(path);
            }
            else
            {
                Console.WriteLine($"File not exist {path}");
                Console.WriteLine("Generate file before start sorting");
            }

            PressAnyKeyDialog();
            await program.NavigateHome(token);
        }

        private static void PressAnyKeyDialog()
        {
            Console.WriteLine("\n\n\nPress any key to go back...");
            Console.ReadKey();
        }

        private static bool CanStartSorting(string path)
        {
            return !string.IsNullOrEmpty(path) && File.Exists(path);
        }

        private static async Task GenerateFileAsync(Program program, UserInputData userData, CancellationToken token)
        {
            await program.NavigateTo<ChoosePathPage>(token);
            Console.Clear();
            await GenerateFileAsync(userData.FilePath, userData.Size);
            PressAnyKeyDialog();
            await program.NavigateHome(token);
        }

        private static async Task GenerateFileAsync(string path, long size)
        {
            var textSize = BytesHelper.GetHumanStringFromBytes(size);
            using var logger = FileGeneratorFactory.GetLogger($"Generate file {textSize}");
            var fileCreator = FileGeneratorFactory.GetFileCreator();
            using var progressReporter = new ProgressReporter(size);
            var file = await fileCreator.GenerateFileAsync(path, size, progressReporter);
            Console.WriteLine($"\nFile generated {file.FullName}");
        }

        private static async Task SortFileAsync(string path)
        {
            using var logger = FileGeneratorFactory.GetLogger($"Sort file");
            var fileSorter = FileGeneratorFactory.GetFileSorter();
            var sorted = await fileSorter.SortFileAsync(path);
            Console.WriteLine($"Sorted file: {sorted}");
        }
    }
}