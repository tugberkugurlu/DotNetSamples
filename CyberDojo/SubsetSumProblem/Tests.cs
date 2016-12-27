using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Theory]
        [InlineData(new int[] { 3, 34, 4, 12, 5, 2 }, 9, true)] // There is a subset (4, 5) with sum 9.
        public void ShouldDoIt(int[] numbers, int sum, bool expected)
        {
            // ACT
            var result = Utils.IsSubsetSum(numbers, sum);

            // ASSERT
            Assert.Equal(expected, result);
        }
    }
}