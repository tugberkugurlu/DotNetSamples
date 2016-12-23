using System.Collections.Generic;
using System.Linq;

namespace CodilityTest
{
    public static class Utils
    {
        public static int CountOfDistinct(int[] A) => A.Distinct().Count();
        public static int CountOfDistinctWithoutLinq(int[] A) => new HashSet<int>(A).Count;
    }
}