
using System;

namespace CodilityTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // BenchmarkTests.RunAndOutput();
            // Utils.FibonacciWithRecursion(10);
            Console.WriteLine(Utils.FibonacciWithDynamicProgramming(1000));
        }
    }
}
