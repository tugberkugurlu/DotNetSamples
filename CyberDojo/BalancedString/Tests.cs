using Xunit;

namespace CodilityTest
{
    /*
        sample: <({()})[]>

        < = add it to the stack
        stack: <

        ( = add it to the stack
        stack: <(

        { = add it to the stack
        stack: <({

        ( = add it to the stack
        stack: <({(

        ) = pop it from stack
        ( = have a look up for it’s closing tag
        ) = is it a match with currentChar? YES!
        stack: <({

        goes on like that…
    */

    public class Tests
    {
        [Theory]
        [InlineData("<[]{()}[]>")]
        [InlineData("<({()})[]>")]
        [InlineData("()[]{}[][]")]
        [InlineData("[()]{}{[()()]()}")]
        [InlineData("{}([]<>)")]
        public void ShouldPass(string value)
        {
            // ACT
            var result = Utils.IsStringBalanced(value);

            // ASSERT
            Assert.True(result);
        }

        [Theory]
        [InlineData("<({([)})[]>")]
        [InlineData("[(])")]
        [InlineData("([}])")]
        [InlineData("{{{{")]
        public void ShouldFail(string value)
        {
            // ACT
            var result = Utils.IsStringBalanced(value);

            // ASSERT
            Assert.False(result);
        }
    }
}