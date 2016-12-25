using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CodilityTest
{
    // see: http://benchmarkdotnet.org/Overview.htm

    public class GetMinimumDiff
    {
        private readonly int[] _aMillionIntegers;

        public GetMinimumDiff()
        {
            var random = new Random();

            // N is an integer within the range [2..100,000];
            // each element of array A is an integer within the range [−1,000..1,000].

            _aMillionIntegers = Enumerable.Range(2, 100000)
                .Select(_ => random.Next(-1000, 1000))
                .ToArray();
        }

        [Benchmark]
        public int OptimizedWith100000Input() => Utils.GetMinimalDifference(_aMillionIntegers);
    }

    public static class BenchmarkTests
    {
        public static void RunAndOutput()
        {
            var summary = BenchmarkRunner.Run<GetMinimumDiff>();
            Console.WriteLine(summary);
        }
    }
}