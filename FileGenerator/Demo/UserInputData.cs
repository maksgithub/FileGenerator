namespace FileGenerator.Demo
{
    public class UserInputData
    {
        public UserInputData(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; set; }
        public long Size { get; set; }
    }
}