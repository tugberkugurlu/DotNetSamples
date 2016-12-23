using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CodilityTest
{
    // see: http://benchmarkdotnet.org/Overview.htm

    public class CountOfDistinct
    {
        private readonly int[] _integers;

        public CountOfDistinct()
        {
            var random = new Random();

            _integers = Enumerable.Range(0, 100000)
                .Select(_ => random.Next(-1000000, 1000000))
                .ToArray();
        }

        [Benchmark]
        public int WithLinq() => Utils.CountOfDistinct(_integers);

        [Benchmark]
        public int WithoutLinq() => Utils.CountOfDistinctWithoutLinq(_integers);
    }

    public static class BenchmarkTests
    {
        public static void RunAndOutput()
        {
            var summary = BenchmarkRunner.Run<CountOfDistinct>();
            
            Console.WriteLine(summary);
        }
    }
}