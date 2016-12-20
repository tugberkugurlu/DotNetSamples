using Xunit;

namespace MostPopularElement
{
    public class Tests
    {
        [Theory]
        [InlineData(new int[] { 1, 3, 4, 5, 2, 2, 3, 2 }, 2, 3)]
        public void ShouldGiveCorrectResult(int[] integers, int frequentNumber, int frequency)
        {
            // ACT
            var result = Utils.FindMostPopularElement(integers);

            // ASSERT
            Assert.Equal(frequentNumber, result.Item);
            Assert.Equal(frequency, result.Count);
        }

        [Theory]
        [InlineData(new int[] { 1, 3, 4, 6, 2, 5, 3, 2, 3, 2 }, 2, 3)]
        [InlineData(new int[] { 10, 20, 10, 9, 7, 12, 1676, 9, 10, 9, 10, 9 }, 9, 4)]
        public void ShouldGiveCorrectLowerNumber_WhenMultipleMatchesWithSameFrequencyFound(int[] integers, int frequentNumber, int frequency)
        {
            // ACT
            var result = Utils.FindMostPopularElement(integers);

            // ASSERT
            Assert.Equal(frequentNumber, result.Item);
            Assert.Equal(frequency, result.Count);
        }
    }
}