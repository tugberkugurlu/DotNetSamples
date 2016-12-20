using System;
using Xunit;

namespace IntegersLeftRightEqual
{
    public class UtilsTests 
    {
        [Theory]
        [InlineData(new long [] { 10, 5, 4, 7, 1, 6, 9, 10, 8 }, 5)]
        [InlineData(new long [] { 1, 3, 4, 4 }, 2)]
        [InlineData(new long [] { 20, 3, 10, 10 }, 1)]
        [InlineData(new long [] { 1, -1, 1 }, 0)]
        [InlineData(new long [] { -1, 10, -9 }, 2)]
        public void GivenCorrectInputsWithMatchingLeftRightEqualTotality_ShouldReturnTheCorrectIndex(long[] integers, int expectedIndex)
        {
            // ACT
            var index = Utils.FindLeftRightEqualTotalityIndex(integers);

            // ASSERT
            Assert.Equal(expectedIndex, index);
        }

        [Fact]
        public void GivenHighInt32Numbers_ShouldHandleInt64InTotal()
        {
            // ARRANGE
            var integers = new [] { long.MaxValue, long.MaxValue, 10, 20 };

            // ACT
            var index = Utils.FindLeftRightEqualTotalityIndex(integers);

            // ASSERT
            Assert.Equal(-1, index);
        }

        [Fact]
        public void GivenANullAsInput_ShouldThrowArgumentNullException()
        {
            // ACT, ASSERT
            var exception = Assert.Throws<ArgumentNullException>(() => Utils.FindLeftRightEqualTotalityIndex(null));
            Assert.Equal("integers", exception.ParamName);
        }

        [Fact]
        public void GivenAnEmptyInt32Array_ShouldReturnZero()
        {
            // ACT
            var index = Utils.FindLeftRightEqualTotalityIndex(Array.Empty<long>());

            // ASSERT
            Assert.Equal(0, index);
        }
    }
}