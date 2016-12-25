using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Theory]
        [InlineData(new int[] { 3, 1, 2, 4, 3 }, 1)]
        [InlineData(new int[] { 1, 5 }, 4)]
        public void ShouldGetCorrentMinDifference(int[] input, int expected)
        {
            // ACT
            var result = Utils.GetMinimalDifference(input);

            // ASSERT
            Assert.Equal(expected, result);
        }
    }
}