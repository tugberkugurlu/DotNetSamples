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

            _aMillionIntegers = Enumerable.Range(0, 100000)
                .Select(_ => random.Next(-1000000, 1000000))
                .ToArray();
        }

        [Benchmark]
        public int WithoutLinq1000000() => Utils.GetMinimalDifference(_aMillionIntegers);
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