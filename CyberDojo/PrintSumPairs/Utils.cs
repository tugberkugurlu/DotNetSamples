using System;
using System.Collections.Generic;

namespace CodilityTest
{
    public static class Utils
    {
        // https://www.careercup.com/question?id=5647545129762816

        public static Tuple<int, int>[] FindSumPairs(int[] numbers)
        {
            if(numbers == null) throw new ArgumentNullException(nameof(numbers));
            if(numbers.Length < 2) throw new ArgumentOutOfRangeException(nameof(numbers));

            var set = new HashSet<int>(numbers);
            var matches = new List<Tuple<int, int>>();

            for (int i = 0; i < numbers.Length; i++)
            {
                var left = numbers[i];
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    var total = (long)left + (long)numbers[j];

                    // we have an array of Int32. So, it cannot possibly contain a value bigger than Int32.MaxValue.
                    if(total < int.MaxValue)
                    {
                        if(set.Contains((int)total))
                        {
                            matches.Add(Tuple.Create(i, j));
                        }
                    }
                }
            }

            return matches.ToArray();
        }
    }
}