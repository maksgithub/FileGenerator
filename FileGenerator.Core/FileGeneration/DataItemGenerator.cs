using System;
using FileGenerator.Core.Common;

namespace FileGenerator.Core.FileGeneration
{
    class DataItemGenerator
    {
        private readonly char[] _duplicateString;
        private readonly int DuplicatesPercentage = 20;
        private readonly Random _random;
        const string Numbers = "0123456789";
        const string LoverChars = " abcdefghijklmnopqrstuvwxyz";
        const string UpperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public DataItemGenerator()
        {
            _random = new Random();
            _duplicateString = new char[_random.Next(15, 350)];
            FillRightSide(_duplicateString, 0);
        }

        public char[] GenerateItem()
        {
            var isDuplicate = _random.Next(0, 100) < DuplicatesPercentage;
            return isDuplicate ? RandomString(_duplicateString) : RandomString(_random.Next(15, 350));
        }

        private char[] RandomString(char[] rightSide)
        {
            var next = _random.Next(100, int.MaxValue);
            var number = next.ToString().ToCharArray();
            var randomString = number.Append(rightSide);
            return randomString;
        }

        private char[] RandomString(int length)
        {
            var result = new char[length];
            var numberLen = _random.Next(1, 10);
            FillLeftSide(result, numberLen);
            FillRightSide(result, numberLen);
            return result;
        }

        private void FillRightSide(char[] result, int numberLength)
        {
            for (int i = numberLength; i < result.Length; i++)
            {
                if (i == numberLength)
                {
                    result[i] = '.';
                }
                else if (i == numberLength + 1)
                {
                    result[i] = ' ';
                }
                else if (i == numberLength + 2)
                {
                    result[i] = UpperChars[_random.Next(UpperChars.Length)];
                }
                else if (result[i - 1] == ' ')
                {
                    result[i] = LoverChars[_random.Next(1, LoverChars.Length)];
                }
                else
                {
                    result[i] = LoverChars[_random.Next(LoverChars.Length)];
                }
            }
        }

        private void FillLeftSide(char[] result, int numberLength)
        {
            for (int i = 0; i < numberLength; i++)
            {
                if (i == 0)
                {
                    result[i] = Numbers[_random.Next(1, Numbers.Length)];
                }
                else
                {
                    result[i] = Numbers[_random.Next(Numbers.Length)];
                }
            }
        }
    }
}