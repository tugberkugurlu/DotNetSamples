using System;
using System.Linq;

namespace CodilityTest
{
    public static class Utils
    {
        // N is an integer within the range [2..100,000];
        // each element of array A is an integer within the range [−1,000..1,000].
        // expected worst-case time complexity is O(N);
        // expected worst-case space complexity is O(N), 

        // Any integer P, such that 0 < P < N, splits this tape into two non-empty parts

        /// <remarks>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/system.math.abs.aspx" />
        /// <para>
        /// (100000 * 1000) < Int32.MaxValue and (100000 * -1000) > Int32.MinValue, so we are fine for overflow cases.
        /// </para>
        /// </remarks>
        public static int GetMinimalDifference(int[] A)
        {
            if(A == null) throw new ArgumentNullException(nameof(A));
            if(A.Length < 2) throw new ArgumentOutOfRangeException(nameof(A));

            if(A.Length == 2)
            {
                return Math.Abs(A[0] - A[1]);
            }

            var minDiff = default(int?);
            for (int i = 1; i < A.Length; i++)
            {
                // this is not ideal to calculate the sume on each run for left and right, can be made better.
                // at least it should be O(log N)

                var left = A.Take(i);
                var right = A.Skip(i);
                var diff = left.Sum(x => x) - right.Sum();
                var absDiff = Math.Abs(diff);

                if(minDiff.HasValue)
                {
                    if(absDiff < minDiff.Value)
                    {
                        minDiff = absDiff;
                    }
                }
                else 
                {
                    minDiff = absDiff;
                }
            }

            return minDiff.Value;
        }
    }
}