using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReverseWords
{
    public class ReverseWordsTests
    {
        [Fact]
        public void Should_Reverse_Words_Correctly_With_Substring()
        {
            string helloWorld = "Hello World";
            int indextOf = helloWorld.IndexOf(' ');
            string hello = helloWorld.Substring(0, indextOf);
            string world = helloWorld.Substring(indextOf + 1, helloWorld.Length - (indextOf + 1));

            Assert.Equal("World Hello", string.Concat(world, " ", hello));
        }

        [Fact]
        public void Should_Reverse_Words_Correctly_With_Split()
        {
            string helloWorld = "Hello World";
            string[] words = helloWorld.Split(new[] { ' ' });

            Assert.Equal("World Hello", string.Concat(words[1], " ", words[0]));
        }

        [Fact]
        public void Should_Reverse_Chars()
        {
            string helloWorld = "Hello World";
            string reversedHelloWorld = new string(helloWorld.Reverse().ToArray());

            Assert.Equal("dlroW olleH", reversedHelloWorld);
        }
    }
}