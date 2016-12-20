using System;
using System.Globalization;
using System.Linq;

namespace IntegersLeftRightEqual
{
    public class Utils
    {
        /*
            Write a function that given an array of integers will return an index where the 
            sum of all the numbers on the left of the index is equal to sum of all the numbers 
            to the right. The function should execute in O(n) time, and O(n) space. If such an 
            index does not exist, return -1. 

            In other words:
            A zero-indexed array A consisting of N integers is given. An equilibrium index of this array is any integer P such that 0 ≤ P < N and the sum of elements of lower indices is equal to the sum of elements of higher indices, i.e. 
            
            A[0] + A[1] + ... + A[P−1] = A[P+1] + ... + A[N−2] + A[N−1].
            
            Sum of zero elements is assumed to be equal to 0. This can happen if P = 0 or if P = N−1.
        */

        // see: http://stackoverflow.com/questions/2799427/what-guarantees-are-there-on-the-run-time-complexity-big-o-of-linq-methods

        /// <remarks>
        /// <para>
        /// The implementation of this method is O(1) space which means that the memory required 
        /// by the algorithm is constant, i.e. does not depend on the size of the input.
        /// </para>
        /// <para>
        /// This method takes the parameters as decimal in order to handle larger values than Int64 on calculation.
        /// We could have also used BigInteger but this should be enough for our needs.
        /// </para>
        /// </remarks>
        public static int FindLeftRightEqualTotalityIndex(long[] integers)
        {
            if(integers == null) 
            {
                throw new ArgumentNullException(nameof(integers));
            }

            if(integers.Length == 0) 
            {
                return 0;
            }

            for(var i = 0; i <= integers.Length; i++)
            {
                var leftSum = integers.Select(x => decimal.Parse(x.ToString(CultureInfo.InvariantCulture))).Take(i).Sum();
                var rightSum = integers.Select(x => decimal.Parse(x.ToString(CultureInfo.InvariantCulture))).Skip(i + 1).Sum();

                if (leftSum == rightSum)
                {
                    return i == integers.Length ? i -1 : i;
                }
            }

            return -1;
        }
    }
}