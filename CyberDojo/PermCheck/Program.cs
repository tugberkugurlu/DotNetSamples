using System;
using System.Collections.Generic;
using System.Linq;

namespace CodilityTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // BenchmarkTests.RunAndOutput();
            var set = new SortedSet<int>(new [] { 10, 2, 15, 25, 2, 10, 100, -19 });
            foreach (var item in Enumerable.Range(set.Min, set.Max))
            {
                System.Console.WriteLine(item);
            }
        }
    }
}
