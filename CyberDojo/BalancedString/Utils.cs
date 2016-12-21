using System.Collections.Generic;

namespace CodilityTest
{
    public class Utils
    {
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
            // We are safe in terms of size of string. So, assume index is int.
            // http://stackoverflow.com/questions/140468/what-is-the-maximum-possible-length-of-a-net-string
            var indicesToSkip = new HashSet<int>();

            for (int i = 0; i < value.Length; i++)
            {
                if(indicesToSkip.Contains(i)) 
                { 
                    continue; 
                }

                var currentCharToCheck = value[i];
                char closePair;

                if(!OpenCloseMappings.TryGetValue(currentCharToCheck, out closePair))
                {
                    return false;
                }
                else
                {
                    var isLast = i == (value.Length - 1);
                    if(isLast)
                    {
                        // If we haven't skipped this index and we are here, return false.
                        return false;
                    }
                    else 
                    {
                        var correspondingCharIndex = value.Length - i - 1;
                        var nextChar = value[i + 1];
                        var correspondingChar = value[correspondingCharIndex];

                        if(nextChar == closePair)
                        {
                            indicesToSkip.Add(i + 1);
                        }
                        else if(correspondingChar == closePair)
                        {
                            indicesToSkip.Add(correspondingCharIndex);
                        }
                        else 
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}