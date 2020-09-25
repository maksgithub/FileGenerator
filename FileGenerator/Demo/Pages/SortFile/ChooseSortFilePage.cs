using System;
using System.IO;
using EasyConsole;
using FileGenerator.Core.Common;

namespace FileGenerator.Demo.Pages.SortFile
{
    public class ChooseSortFilePage : MenuPage
    {
        public ChooseSortFilePage(Program program, UserInputData userData) : base("Choose sort file", program)
        {
            Menu.AddSync($"Start sort: {userData.FilePath}", () => { });
            Menu.AddSync($"Start sort custom", () => SetPath(userData));
        }

        private void SetPath(UserInputData userData)
        {
            while (true)
            {
                var path = Input.ReadString("Set file path:");
                var file = FileSystemHelper.CreateFile(path.Trim('"').Trim());
                if (file?.Directory?.Exists ?? false)
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