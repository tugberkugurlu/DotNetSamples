using System;
using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Theory]
        [InlineData(new int[] { 1, 3, 6, 10, 11, 15 }, 2)]
        [InlineData(new int[] { 1, 1, 1, 1 }, 5)]
        [InlineData(new int[] { 1, 2, 5, 10, 20, 40 }, 4)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6 }, 22)]
        public void ShouldMultiplyCorrectly(int[] input, int expected)
        {
            // ACT
            var result = Utils.FindSmallestPositiveInt(input);

            // ASSERT
            Assert.Equal(expected, result);
        }
    }
}