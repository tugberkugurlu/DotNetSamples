using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CodilityTest
{
    // see: http://benchmarkdotnet.org/Overview.htm

    public class SimpleBenchmark
    {
        public SimpleBenchmark()
        {
        }

        [Benchmark]
        public void Simple() 
        {
        }
    }

    public static class BenchmarkTests
    {
        public static void RunAndOutput()
        {
            var summary = BenchmarkRunner.Run<SimpleBenchmark>();
            Console.WriteLine(summary);
        }
    }
}