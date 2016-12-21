using System;
using System.Globalization;
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

        // PermMissingElem
        // https://codility.com/demo/results/trainingP7CBEF-J6J/#task-0
        // http://codereview.stackexchange.com/a/129277/9622

        public static int FindMissing(int[] A)
        {
            var n = A.Length + 1;
            var sumOfAllElements = (decimal.Parse(n.ToString(CultureInfo.InvariantCulture)) * (1 + n)) / 2;
            var sumOfCurrent = A.Sum(x => decimal.Parse(x.ToString(CultureInfo.InvariantCulture)));
            return (int)(sumOfAllElements - sumOfCurrent);
        }
    }
}