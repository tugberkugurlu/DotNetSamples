using System;
using System.Collections.Generic;
using System.Linq;

namespace CodilityTest
{
    public static class Utils
    {
        /*
            see: http://www.geeksforgeeks.org/dynamic-programming-subset-sum-problem/

            Given a set of non-negative integers, and a value sum, 
            determine if there is a subset of the given set with sum equal to given sum.

            Examples: set[] = {3, 34, 4, 12, 5, 2}, sum = 9
            Output:  True  //There is a subset (4, 5) with sum 9.
        */

        public static bool IsSubsetSum(int[] set, int sum)
        {
            // base case
            if(sum == 0) 
            {
                return true;
            }

            if(set.Length == 0 && sum != 0)
            {
                return false;
            }

            // If last element is greater than sum, then ignore it
            if(set.Last() > sum)
            {
                return IsSubsetSum(set.Take(set.Length - 1).ToArray(), sum);
            }

            var newSubset = set.Take(set.Length -1).ToArray();

            return IsSubsetSum(newSubset, sum) ||
                IsSubsetSum(newSubset, sum - set.Last());
        }
    }
}