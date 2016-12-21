using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Theory]
        [InlineData("<[]{()}[]>")]
        [InlineData("<({()})[]>")]
        public void ShouldPass(string value)
        {
            // ACT
            var result = Utils.IsStringBalanced(value);

            // ASSERT
            Assert.True(result);
        }

        [Theory]
        [InlineData("<({([)})[]>")]
        public void ShouldFail(string value)
        {
            // ACT
            var result = Utils.IsStringBalanced(value);

            // ASSERT
            Assert.False(result);
        }
    }
}