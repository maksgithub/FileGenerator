using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileGenerator.Core.Common;
using FileGenerator.Core.Common.Comparers;
using NUnit.Framework;

namespace FileGeneratorTests
{
    [TestFixture]
    public class BinaryDataItemComparerTests
    {
        [TestCase("1. Apple", "415. Apple", -1)]
        [TestCase("41. Apple", "415. Apple", -1)]
        [TestCase("415. Apple", "1. Apple", 1)]
        [TestCase("222. Apple", "223. Apple", -1)]
        [TestCase("226. Apple", "223. Apple", 3)]
        [TestCase("12345678. Apple", "1234567. Apple", 1)]
        [TestCase("12345678. Apple", "123456789. Apple", -1)]
        [TestCase("123456789. Apple", "123456789. Apple", 0)]
        [TestCase("1. Apple", "0. Apple", 1)]
        [TestCase("2. Banana is yellow", "32. Cherry is the best", -1)]
        [TestCase("0. Windows", "32. Cherry is the best", 20)]
        [TestCase("42. Banana is yellow", "415. Apple", 1)]
        [TestCase(". Banana is yellow", ". Apple", 1)]
        [TestCase("2. Banana is yellow", "32. Cherry is the best", -1)]
        [TestCase("2. Banana is yellow2.", "2. Banana is yellow2.", 0)]
        [TestCase("4. A", "42. B", -1)]
        [TestCase("42. A", "42. B", -1)]

        public void DataComparerTest(string x, string y, int expected)
        {
            var dataComparer = new StringDataItemComparer();
            var actual = dataComparer.Compare(x, y);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DataComparerTest_SortList_Compare_Words()
        {
            var dataComparer = new StringDataItemComparer();
            var list = new List<string>()
            {
                "4. A",
                "42. B"
            };

            list.Sort(dataComparer);
            
            CollectionAssert.AreEqual(new List<string>()
            {
                "4. A",
                "42. B"
            }, list);
        }  
        
        [Test]
        public void DataComparerTest_SortList_Compare_Numbers()
        {
            var dataComparer = new StringDataItemComparer();
            var list = new List<string>()
            {
                "415. Same",
                "1. Same",
                "3. Same",
                "2. Same",
                "5. Same",
                "4. Same",
                "41. Same",
            };

            list.Sort(dataComparer);

            CollectionAssert.AreEqual(new List<string>()
            {
                "1. Same",
                "2. Same",
                "3. Same",
                "4. Same",
                "5. Same",
                "41. Same",
                "415. Same",
            }, list);
        }

        [Test]
        public void DataComparerTest_SortList()
        {
            var dataComparer = new StringDataItemComparer();
            var list = new List<string>()
            {
                "415. Apple",
                "30432. Something something something",
                "32. Cherry is the best",
                "1. Apple",
                "2. Banana is yellow"
            };

            list.Sort(dataComparer);

            CollectionAssert.AreEqual(new List<string>()
            {
                "1. Apple",
                "415. Apple",
                "2. Banana is yellow",
                "32. Cherry is the best",
                "30432. Something something something",
            }, list);
        }

        [Test]
        public void DataComparerTest_SortList_2()
        {
            var dataComparer = new StringDataItemComparer();
            var list = new List<string>()
            {
                "415. Bpple",
                "1. Apple",
            };

            list.Sort(dataComparer);

            CollectionAssert.AreEqual(new List<string>()
            {
                "1. Apple",
                "415. Bpple",
            }, list);
        }
    }
}
