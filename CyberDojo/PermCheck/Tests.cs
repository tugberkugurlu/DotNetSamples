using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Theory]
        [InlineData(new int[] { 3, 4 }, 1)]
        [InlineData(new int[] { 3, 5 }, 0)]
        public void ShouldDoIt(int[] input, int expected)
        {
            var result = Utils.IsPerm(input);
            Assert.Equal(expected, result);
        }
    }
}