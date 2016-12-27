using System;
using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Fact]
        public void ShouldGetCorrectResult()
        {
            var input = new int[] { 20, 1, 20, 12, 12, 10, 5, 7, 22 };
            var expected = new Tuple<int, int>[] 
            {
                Tuple.Create(3, 5),
                Tuple.Create(4, 5),
                Tuple.Create(6, 7)
            };

            // ACT
            var result = Utils.FindSumPairs(input);

            // ASSERT
            Assert.Equal(expected.Length, result.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        [Fact]
        public void ShouldHandleOverflowCasesCorrectly()
        {
            var input = new int[] { int.MaxValue, 1, 22, 12, 10 };
            var expected = new Tuple<int, int>[] 
            {
                Tuple.Create(3, 4)
            };

            // ACT
            var result = Utils.FindSumPairs(input);

            // ASSERT
            Assert.Equal(expected.Length, result.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }
    }
}