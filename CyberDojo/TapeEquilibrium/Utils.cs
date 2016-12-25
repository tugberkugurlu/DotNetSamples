using System;

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

            int leftSum = A[0];
            int rightSum = 0;

            for (int i = 1; i < A.Length; i++)
            {
                rightSum += A[i];
            }

            int absDiff = Math.Abs(leftSum - rightSum);

            for (int i = 1; i < (A.Length - 1); i++)
            {
                var currentValue = A[i];

                leftSum += currentValue;
                rightSum -= currentValue;

                var currentAbsDiff = Math.Abs(leftSum - rightSum);

                if(currentAbsDiff < absDiff)
                {
                    absDiff = currentAbsDiff;
                }
            }

            return absDiff;

            /*
                This is the first solution I came up with which was really bad in terms of perf.
                Benchmark tests showed that it was not really O(N).
                I realised that I really didn't have to resum every time inside the loop. Instead,
                I could have only done it once and subtract from there. I realise that this is 
                still not O(N) but it terms of perf, it's a bit better.
                
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
            */
        }
    }
}