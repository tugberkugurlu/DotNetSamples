using System;
using System.Linq;

namespace MostPopularElement
{
    public static class Utils
    {
        /*
            Given an integer array, find the most frequent number and it's count in the array. 
            Write the code in O(1) space. Eg 1 , 3, 4, 5, 2, 2, 3, 2 Output Most frequent number is 2. 
            The frequency is 3. Return the output as string in 'number: frequency' format. e.g. 
            2: 3 (Please note the space after : and frequency. If multiple numbers have the same highest 
            frequency return the smallest number.
        */

        public static MostPoularItem FindMostPopularElement(int[] integers)
        {
            if(integers == null) 
            {
                throw new ArgumentNullException(nameof(integers));
            }

            return integers.GroupBy(x => x)
                .OrderBy(x => x.Key)
                .OrderByDescending(x => x.Count())
                .Select(x => new MostPoularItem(x.Key, x.Count()))
                .FirstOrDefault();
        }
    }

    public struct MostPoularItem
    {
        public MostPoularItem(int item, int count)
        {
            Item = item;
            Count = count;
        }

        public int Item { get; }
        public int Count { get; }
    }
}