using System;

namespace CodilityTest
{
    public static class Utils
    {
        /*
            see: http://www.geeksforgeeks.org/find-smallest-value-represented-sum-subset-given-array/

            Given a sorted array (sorted in non-decreasing order) of positive numbers, find the smallest positive integer value that cannot be represented as sum of elements of any subset of given set. 
            Expected time complexity is O(n).
        */

        public static int FindSmallestPositiveInt(int[] numbers)
        {
            if(numbers == null) throw new ArgumentNullException(nameof(numbers));

            var potentialMin = 1;

            for (int i = 0; i < numbers.Length && numbers[i] <= potentialMin; i++)
            {
                potentialMin = potentialMin + numbers[i];
            }

            return potentialMin;
        }
    }
}