using System.Linq;

namespace CodilityTest
{
    public class Utils
    {
        /*
            Assume that:
            N is an odd integer within the range [1..1,000,000];
            each element of array A is an integer within the range [1..1,000,000,000];
            all but one of the values in A occur an even number of times.

            Complexity:
            expected worst-case time complexity is O(N);
            expected worst-case space complexity is O(1), beyond input storage (not counting the storage required for input arguments).
        */

        public static int FindUnPairedNumber(int[] A) 
        {
            return A.GroupBy(x => x)
                .Select(group => new { Item = group.Key, Occurance = group.Count() })
                .First(x => x.Occurance % 2 != 0).Item;
        }
    }
}