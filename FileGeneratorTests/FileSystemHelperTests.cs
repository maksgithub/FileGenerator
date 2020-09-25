using System.IO;
using System.Text;
using FileGenerator.Core.Common;
using NUnit.Framework;

namespace FileGeneratorTests
{
    [TestFixture]
    public class FileSystemHelperTests
    {
        [TestCase("Line\r\n", "Line")]
        [TestCase("\r\n", "")]
        [TestCase("line\r\n\r\n\r\n\r\n", "line")]
        public void RemoveEmptyLineTest(string input, string expected)
        {
            var buffer = Encoding.ASCII.GetBytes(input);
            using var stream = new MemoryStream(buffer);
            FileSystemHelper.RemoveNewLineFromEnd(stream);

            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            var actual = reader.ReadToEnd();

            //Assert.That(actual, Has.Length.EqualTo(expected.Length));
            Assert.AreEqual(expected, actual);
        }
    }
}