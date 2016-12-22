using Xunit;

namespace FrogJumps
{
    public class Tests
    {
        [Theory]
        [InlineData(1, 10, 1, 9)]
        [InlineData(10, 85, 30, 3)]
        public void ShouldPass(int current, int target, int jumpPower, int expected)
        {
            var result = Utils.MinimumSteps(current, target, jumpPower);
            Assert.Equal(expected, result);
        }
    }
}