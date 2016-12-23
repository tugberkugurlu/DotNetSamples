using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CodilityTest
{
    // see: http://benchmarkdotnet.org/Overview.htm

    public class CountOfDistinct
    {
        private readonly int[] _aHundredIntegers;
        private readonly int[] _tenThousandsIntegers;
        private readonly int[] _aHundredThousandsIntegers;
        private readonly int[] _fiveHundredThousandsIntegers;
        private readonly int[] _aMillionIntegers;

        public CountOfDistinct()
        {
            var random = new Random();

            _aHundredIntegers = Enumerable.Range(0, 100)
                .Select(_ => random.Next(-1000000, 1000000))
                .ToArray();

            _tenThousandsIntegers = Enumerable.Range(0, 10000)
                .Select(_ => random.Next(-1000000, 1000000))
                .ToArray();

            _aHundredThousandsIntegers = Enumerable.Range(0, 100000)
                .Select(_ => random.Next(-1000000, 1000000))
                .ToArray();

            _fiveHundredThousandsIntegers = Enumerable.Range(0, 500000)
                .Select(_ => random.Next(-1000000, 1000000))
                .ToArray();

            _aMillionIntegers = Enumerable.Range(0, 100000)
                .Select(_ => random.Next(-1000000, 1000000))
                .ToArray();
        }

        [Benchmark]
        public int WithLinq100() => Utils.CountOfDistinct(_aHundredIntegers);

        [Benchmark]
        public int WithoutLinq100() => Utils.CountOfDistinctWithoutLinq(_aHundredIntegers);

        [Benchmark]
        public int WithLinq10000() => Utils.CountOfDistinct(_tenThousandsIntegers);

        [Benchmark]
        public int WithoutLinq10000() => Utils.CountOfDistinctWithoutLinq(_tenThousandsIntegers);

        [Benchmark]
        public int WithLinq100000() => Utils.CountOfDistinct(_aHundredThousandsIntegers);

        [Benchmark]
        public int WithoutLinq100000() => Utils.CountOfDistinctWithoutLinq(_aHundredThousandsIntegers);

        [Benchmark]
        public int WithLinq500000() => Utils.CountOfDistinct(_fiveHundredThousandsIntegers);

        [Benchmark]
        public int WithoutLinq500000() => Utils.CountOfDistinctWithoutLinq(_fiveHundredThousandsIntegers);

        [Benchmark]
        public int WithLinq1000000() => Utils.CountOfDistinct(_aMillionIntegers);

        [Benchmark]
        public int WithoutLinq1000000() => Utils.CountOfDistinctWithoutLinq(_aMillionIntegers);
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