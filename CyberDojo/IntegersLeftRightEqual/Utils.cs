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
        */

        public static int FindLeftRightEqualTotalityIndex(int[] integers)
        {
            for(var i = 1; i < integers.Length; i++)
            {
                var left = integers.Take(i);
                var right = integers.Skip(i + 1);

                if (left.Sum() == right.Sum())
                {
                    return i;
                }
            }

            return -1;
        }
    }
}