using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodilityTest
{
    public class Tests
    {
        [Theory]
        [InlineData("abcdaf", "acbcf", 4)] // 
        [InlineData("zhjuioms", "juwhdhibms", 5)]
        public void ShouldOuputCorrectlyIt(string str1, string str2, int count)
        {
            // ACT
            var result = Utils.CountOfCommonLongestSubsequence(str1, str2);

            // ASSERT
            Assert.Equal(count, result);
        }
    }
}