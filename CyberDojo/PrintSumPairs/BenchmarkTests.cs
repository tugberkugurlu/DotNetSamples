using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CodilityTest
{
    // see: http://benchmarkdotnet.org/Overview.htm

    public class FindSumPairsWith10000
    {
        private readonly int[] _numbers;

        public FindSumPairsWith10000()
        {
            var random = new Random();

            _numbers = Enumerable.Range(0, 10000)
                .Select(_ => random.Next(0, int.MaxValue))
                .ToArray();
        }

        [Benchmark]
        public void For1000000() 
        {
            Utils.FindSumPairs(_numbers);
        }
    }

    public static class BenchmarkTests
    {
        public static void RunAndOutput()
        {
            var summary = BenchmarkRunner.Run<FindSumPairsWith10000>();
            Console.WriteLine(summary);
        }
    }
}