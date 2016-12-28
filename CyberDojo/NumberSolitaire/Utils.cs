using System;
using System.Collections.Generic;
using System.Linq;

namespace CodilityTest
{
    public static class Utils
    {
        /*
            original problem: https://codility.com/programmers/lessons/17-dynamic_programming/number_solitaire/
            see: http://blog.nkbits.com/2015/07/15/codility-number-solitaire/
            submission: https://codility.com/demo/results/training56A3U5-MGH/

            Demonstration: First let's consider this, in a array A with size N, index i and k being the 
            dice result, we have a tree of possibilities that could be described as follows. 
            
            Starting from 0 we could go to indexes 1, 2, 3, 4, 5, 6, and them from 1, we could go to 
            2, 3, 4, 5, 6, 7, so basically from index i, we can go from i + 1, to i + 6, as long as i + 6 < N. 
            So to calculate the total of possible combinations let's traverse the array from N - 1, to 0. 
            
            Starting at N - 1, we have no possible combination, at N - 2, we have 1 combination with k = 1, 
            from N - 3, we can get to N - 2, with k = 1, and N - 1 with k = 2, which gives 1 combination when 
            we reach N - 1, and from N - 2 we know that we cant get only one combination, so this gives us 2 
            possibilities. The pattern follows on the table below. 



            We can see that there is a pattern here, for an array of size N the total of combinations is 
            2N−22N−2, but this pattern doesn't continue for too long, because we have a die with 
            six sides when N >= 8 we start to lose some combinations, for example, when N = 8, N - 1 = 7, 
            and we can't get from 0 to 7 with the values that can be assumed by k, so the total combinations 
            for N = 8 would be 2N−2−12N−2−1. For N = 9, we are not going to be able to reach from 0 indexes 
            7, 8, Losing two combinations, as you can see, this is going to be very similar to the pattern 
            that we found before. For N≥8N≥8 we have to a total combinations of 2N−2−(2N−8)2N−2−(2N−8). This 
            is not a strict proof.
        */

        // N is an integer within the range [2..100,000];
        // each element of array A is an integer within the range [−10,000..10,000].

        /// <remarks>
        /// The strategy to solve this using a Greedy Algorithm.
        /// </remarks>
        /// <seealso href="http://blog.nkbits.com/2015/07/15/codility-number-solitaire/"></seealso>
        /// <seealso href="https://github.com/GNakayama/codility/blob/master/src/python/lesson15/exercise1.py"></seealso>
        public static int MaxPossibleResultNumberSolitaire(int[] A)
        {
            if(A == null) throw new ArgumentNullException(nameof(A));
            if(A.Length < 2) throw new ArgumentOutOfRangeException(nameof(A));
            if(A.Length > 100000) throw new ArgumentOutOfRangeException(nameof(A));

            // This would effect the perf. So, not do it just for sanity sake.
            // if(A.Any(x => x < -10000 || x > 10000)) throw new ArgumentOutOfRangeException(nameof(A));

            // If we have only two elements the result should be the sum of this elements
            if(A.Length == 2)
            {
                return A[0] + A[1];
            }

            // First we have to realize that with a six side dice, from index i we can only reach 
            // from i + 1 to i + 6, so we are going to declare an array of size 7, where we are going 
            // to keep our results, the index 0 is going to store our current result and the rest 
            // will store the values of our next move relative to index i.

            var result = new int[7];
            result[0] = A[0];

            for (int dice = 0; dice < 7; dice++)
            {
                if(dice < A.Length)
                {
                    result[dice] = A[0] + A[dice];
                }   
            }

            for (int k = 1; k < A.Length; k++)
            {
                result[0] = result[1];

                for (int dice = 1; dice < 6; dice++)
                {
                    if(k + dice < A.Length)
                    {
                        result[dice] = Math.Max(result[dice + 1], result[0] + A[k + dice]);
                    }
                    else 
                    {
                        break;
                    }
                }

                if(k + 6 < A.Length)
                {
                    result[6] = result[0] + A[k + 6];
                }
            }

            return result[0];
        }
    }
}