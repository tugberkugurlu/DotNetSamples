using Xunit;

namespace IntegersLeftRightEqual
{
    public class UtilsTests 
    {
        [Theory]
        [InlineData(new [] { 10, 5, 4, 7, 1, 6, 9, 10, 8 }, 5)]
        [InlineData(new [] { 1, 3, 4, 4 }, 2)]
        [InlineData(new [] { 20, 3, 10, 10 }, 1)]
        public void GivenCorrectInputsWithMatchingLeftRightEqualTotality_ShouldReturnTheCorrectIndex(int[] integers, int expectedIndex)
        {
            // ACT
            var index = Utils.FindLeftRightEqualTotalityIndex(integers);

            // ASSERT
            Assert.Equal(expectedIndex, index);
        }
    }
}