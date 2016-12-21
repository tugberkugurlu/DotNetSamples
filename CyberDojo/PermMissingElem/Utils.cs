using System;
using System.Linq;

namespace CodilityTest
{
    public class Utils
    {
        /*
            Assume that:
            N is an integer within the range [0..100,000];
            the elements of A are all distinct;
            each element of array A is an integer within the range [1..(N + 1)].

            Complexity:
            expected worst-case time complexity is O(N);
            expected worst-case space complexity is O(1), beyond input storage (not counting the storage required for input arguments).
        */

        public static int FindMissing(int[] A)
        {
            var sortedList = A.OrderByDescending(x => x);

            if(sortedList.Last() != 1)
            {
                return 1;
            }

            for (int i = 0; i < A.Length; i++)
            {
                var current = sortedList.ElementAt(i);
                if((current - sortedList.ElementAt(i + 1)) > 1)
                {
                    return current - 1;
                }
            }

            throw new NotSupportedException();
        }
    }
}