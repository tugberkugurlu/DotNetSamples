using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public class Foo 
        {
            public int Growth { get; set; }
            public int Current { get; set; }
        }

        /*
            q2: 20, q1: 10
            g: q2 - q1 = 10

            10 10
            100
        */

        public static void Main(string[] args)
        {
            var data = new [] { 15, 30, 10, 10, 0, 10, 15 };
            var expected = new [] { 50, -200, 0, -100, 100, 33.33 };
            var result = CalculateGrowth(data);

            foreach (var item in result)
            {
                System.Console.WriteLine(item);
            }
        }

        public static IEnumerable<double> CalculateGrowth(IEnumerable<int> data) 
        {                        
            return data.Skip(1).Zip(data, (x, y) => 
            {
                var result = Math.Round(((double)(x - y) / x) * 100, 2);
                return result == Double.NegativeInfinity ? -100 : result;
            });
        }
    }
}