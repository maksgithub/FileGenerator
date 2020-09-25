namespace FileGeneratorTests.Common
{
    public class DataStorage
    {
        public static string InputData1 { get; } = 
@"415. Apple
2. Banana is yellow
30432. Something something something
1. Apple
32. Cherry is the best
0. Windows
2. Banana is yellow";

        public static string ExpectedData1 =>
@"1. Apple
415. Apple
2. Banana is yellow
2. Banana is yellow
32. Cherry is the best
30432. Something something something
0. Windows";

        public static string InputData2 => "2. Banana is yellow\r\n2. Banana is yellow";
        public static string ExpectedData2 => "2. Banana is yellow\r\n2. Banana is yellow";

        public static string InputData3 => "2. Banana is y";
        public static string ExpectedData3 => "2. Banana is y";

        public static string InputData4 =>
@"415. Apple
1. Apple";
        public static string ExpectedData4 =>
@"1. Apple
415. Apple";
    }
}