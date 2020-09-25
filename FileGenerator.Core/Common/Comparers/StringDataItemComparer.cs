using System;
using System.Collections.Generic;

namespace FileGenerator.Core.Common.Comparers
{
    class StringDataItemComparer : IComparer<string>
    {
        public int Compare(string xData, string yData)
        {
            if (xData == null || yData == null)
            {
                return -1;
            }

            var xDot = xData.IndexOf('.');
            var xRight = xData.AsSpan(xDot, xData.Length - xDot);

            var yDot = yData.IndexOf('.');
            var yRight = yData.AsSpan(yDot, yData.Length - yDot);

            var result = xRight.CompareTo(yRight, StringComparison.Ordinal);
            if (result == 0)
            {
                result = xDot.CompareTo(yDot);
                if (result == 0)
                {
                    var xLeft = xData.AsSpan(0, xDot);
                    var yLeft = yData.AsSpan(0, yDot);
                    result = xLeft.CompareTo(yLeft, StringComparison.Ordinal);
                }
            }
            
            return result;
        }
    }
}