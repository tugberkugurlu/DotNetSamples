using System;

namespace CodilityTest
{
    public static class Utils
    {
        /*
            from: https://en.wikipedia.org/wiki/Prefix_sum

            In computer science, the prefix sum, cumulative sum, inclusive scan, or simply scan of a sequence of numbers x0, x1, x2, ... is a second sequence of numbers y0, y1, y2, ..., the sums of prefixes (running totals) of the input sequence:

            y0 = x0
            y1 = x0 + x1
            y2 = x0 + x1+ x2

            For instance, the prefix sums of the natural numbers are the triangular numbers:

            input numbers	 1	 2	 3	 4	 5	 6	...
            prefix sums	 1	 3	 6	10	15	21	...
        */

        // N is an integer within the range [1..100,000];
        // each element of array A is an integer that can have one of the following values: 0, 1.
        public static int CountPassingCars(int[] A)
        {
            if(A == null) new ArgumentNullException(nameof(A));
            if(A.Length < 1 || A.Length > 100000) new ArgumentOutOfRangeException(nameof(A));

            int count = 0;
            int multiply = 0;

            foreach (var car in A)
            {
                if(car == 0)
                {
                    multiply = multiply + 1;
                }

                if(multiply > 1)
                {
                    if(car == 1)
                    {
                        count = count + multiply;
                        if(count > 1000000000)
                        {
                            return -1;
                        }
                    }
                }
            }

            return count;
        }

        // see: https://github.com/apaleyes/spac.net/blob/5950afee060a15c8ea91e05261a07c6ebbaab300/sources/spac.net/spac.net/Chapter5/SequentialPrefixSum.cs
        public static int[] PrefixSum(int[] A)
        {
            var output = new int[A.Length];
            output[0] = A[0];

            for (int i = 1; i < A.Length; i++)
            {
                output[i] = output[i-1] + A[i];
            }

            return output;
        }
    }
}