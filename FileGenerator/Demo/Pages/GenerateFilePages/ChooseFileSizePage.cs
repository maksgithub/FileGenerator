using EasyConsole;

namespace FileGenerator.Demo.Pages.GenerateFilePages
{

    internal class ChooseFileSizePage : MenuPage
    {
        private const int DefaultSizeMb = 100;
        private readonly UserInputData _userData;

        public ChooseFileSizePage(Program program, UserInputData userData) : base("Choose file size", program)
        {
            _userData = userData;
            Menu.AddSync("Default size (100 MB)", SetDefaultSize);
            Menu.AddSync("Custom size", SetUserSize);
        }

        private void SetUserSize()
        {
            var sizeInMb = Input.ReadInt("Please enter file size in MB (between 1 and 200000):", min: 1, max: 200000);
            _userData.Size = ToBytes(sizeInMb);
        }

        private void SetDefaultSize()
        {
            _userData.Size = ToBytes(DefaultSizeMb);
        }

        private static int ToBytes(int sizeInBytes)
        {
            return sizeInBytes * 1024 * 1024;
        }
    }
}