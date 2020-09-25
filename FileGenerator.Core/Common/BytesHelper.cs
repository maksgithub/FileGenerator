namespace FileGenerator.Core.Common
{
    public static class BytesHelper
    {
        public static string GetHumanStringFromBytes(double length)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (length >= 1024 && order < sizes.Length - 1)
            {
                order++;
                length = length / 1024;
            }
            return  $"{length:0.##} {sizes[order]}";
        }
    }
}