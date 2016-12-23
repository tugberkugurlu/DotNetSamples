using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Theory]
        [InlineData(new int[] { 2, 2, 1, 3, 1, 1 }, 3)]
        [InlineData(new int[] { 5, 1, 2, 1, 5 }, 3)]
        [InlineData(new int[] { 1 }, 1)]
        public void ShouldFindCorrectNumberOfDistinctValues(int[] integers, int expected)
        {
            // ACT
            var result = Utils.CountOfDistinct(integers);

            // ASSERT
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new int[] { 2, 2, 1, 3, 1, 1 }, 3)]
        [InlineData(new int[] { 5, 1, 2, 1, 5 }, 3)]
        [InlineData(new int[] { 1 }, 1)]
        public void ShouldFindCorrectNumberOfDistinctValuesWithoutLinq(int[] integers, int expected)
        {
            // ACT
            var result = Utils.CountOfDistinctWithoutLinq(integers);

            // ASSERT
            Assert.Equal(expected, result);
        }
    }
}