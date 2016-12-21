using System.Collections.Generic;

namespace CodilityTest
{
    public class Utils
    {
        // http://codereview.stackexchange.com/questions/45916/check-for-balanced-parentheses
        // http://stackoverflow.com/questions/140468/what-is-the-maximum-possible-length-of-a-net-string

        private static readonly IDictionary<char, char> OpenCloseMappings = new Dictionary<char, char>
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' }
        };

        public static bool IsStringBalanced(string value)
        {
            return value.Length % 2 == 0 
                ? IsStringBalancedImpl(value) 
                : false;
        }

        private static bool IsStringBalancedImpl(string value)
        {
            var stack = new Stack<char>();
            for (int i = 0; i < value.Length; i++)
            {
                var currentChar = value[i];
                if(OpenCloseMappings.ContainsKey(currentChar))
                {
                    stack.Push(currentChar);
                }
                else if (stack.Count == 0 || currentChar != OpenCloseMappings[stack.Pop()])
                {
                    return false;
                }
            }

            return true;
        }
    }
}