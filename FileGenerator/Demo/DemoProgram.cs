using System;
using System.IO;
using EasyConsole;
using FileGenerator.Demo.Pages;
using FileGenerator.Demo.Pages.GenerateFilePages;
using FileGenerator.Demo.Pages.SortFile;

namespace FileGenerator.Demo
{
    class DemoProgram : Program
    {
        public DemoProgram() : base("Main", breadcrumbHeader: true)
        {
            var userData = new UserInputData();
            AddPage(new MainPage(this, userData));
            AddPage(new ChoosePathPage(this, userData));
            AddPage(new ChooseFileSizePage(this, userData));
            AddPage(new ChooseSortFilePage(this, userData));

            SetPage<MainPage>();
        }
    }
}