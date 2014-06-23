using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YieldSample
{
    class Program
    {
        private static long CallCount = 0;

        static void Main(string[] args)
        {
            IEnumerable<long> firstTen = GetNumbersLazy().Take(10).ToList();
            Console.WriteLine(CallCount);
            CallCount = 0;

            IEnumerable<long> skipTenGetNextTen = GetNumbersLazy().Skip(10).Take(10).ToList();
            Console.WriteLine(CallCount);
            CallCount = 0;

            long count = GetNumbers().Count();
            Console.WriteLine(CallCount);
            CallCount = 0;

            long countLazy = GetNumbersLazy().Count();
            Console.WriteLine(CallCount);
            CallCount = 0;

            Console.ReadLine();
        }

        static IEnumerable<long> GetNumbersLazy()
        {
            for (long i = 0; i < long.MaxValue; i++)
            {
                Interlocked.Increment(ref CallCount);
                yield return i;
            }
        }

        static IEnumerable<long> GetNumbers()
        {
            List<long> values = new List<long>();
            for (long i = 0; i < long.MaxValue; i++)
            {
                values.Add(i);
            }

            return values;
        } 
    }
}