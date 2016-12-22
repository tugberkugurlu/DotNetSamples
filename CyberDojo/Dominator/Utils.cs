using System;
using System.Linq;

namespace CodilityTest
{
    public static class Utils
    {
        public static int FindDominator(int[] A)
        {
            var item = A.GroupBy(x => x)
                .Select(group => new { Item = group.Key, Occurance = group.Count() })
                .Where(x => x.Occurance > A.Length / 2)
                .OrderByDescending(x => x.Occurance)
                .FirstOrDefault();

            return item != null ? Array.FindIndex(A, x => x == item.Item) : -1;
        }
    }
}