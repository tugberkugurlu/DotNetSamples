using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CodilityTest
{
    // see: http://benchmarkdotnet.org/Overview.htm

    public class Multiply
    {
        private readonly int[] _aMillionIntegers;

        public Multiply()
        {
            var random = new Random();

            _aMillionIntegers = Enumerable.Range(0, 100000)
                .Select(_ => random.Next(-1000000, 1000000))
                .ToArray();
        }

        [Benchmark]
        public void For1000000() => Utils.Multiply(_aMillionIntegers.First());
    }

    public static class BenchmarkTests
    {
        public static void RunAndOutput()
        {
            var summary = BenchmarkRunner.Run<Multiply>();
            Console.WriteLine(summary);
        }
    }
}