using System;
using System.IO;
using EasyConsole;
using FileGenerator.Core.Common;

namespace FileGenerator.Demo.Pages.GenerateFilePages
{
    public class ChoosePathPage : MenuPage
    {
        private readonly Program _program;

        public ChoosePathPage(Program program, UserInputData userData) : base("Choose path", program)
        {
            _program = program;
            Menu.Add($"Default path ({userData.FilePath})", program.NavigateTo<ChooseFileSizePage>);
            Menu.Add($"Custom path", token =>
            {
                SetCustomPath(userData);
                return program.NavigateTo<ChooseFileSizePage>(token);
            });
        }

        private void SetCustomPath(UserInputData userData)
        {
            while (true)
            {
                var path = Input.ReadString("Set file path:");
                var file = FileSystemHelper.CreateFile(path.Trim('"').Trim());
                if (file != null)
                {
                    userData.FilePath = file.FullName;
                    break;
                }
                else
                {
                    Console.WriteLine("Path not valid or directory not exist");
                }
            }
        }
    }
}