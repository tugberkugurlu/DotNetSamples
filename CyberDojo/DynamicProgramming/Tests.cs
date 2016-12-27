using System;
using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Theory]
        [InlineData(10, 20)]
        [InlineData(Int32.MaxValue, 4294967294)]
        public void ShouldMultiplyCorrectly(int input, long expected)
        {
        }
    }
}