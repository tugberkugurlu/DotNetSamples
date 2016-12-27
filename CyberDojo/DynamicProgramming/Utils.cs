using System;
using System.Collections.Generic;

namespace CodilityTest
{
    public static class Utils
    {
        // http://www.geeksforgeeks.org/dynamic-programming-set-13-cutting-a-rod/
        // http://www.geeksforgeeks.org/fundamentals-of-algorithms/#DynamicProgramming
        // https://www.youtube.com/watch?v=sF7hzgUW5uY

        public static int CutRod(int[] price, int n)
        {
            int[] val = new int[n+1];
            val[0] = 0;

            for (int i = 1; i < n; i++)
            {
                int maxVal = int.MinValue;
                for (int j = 0; j < i; j++)
                {
                    maxVal = Math.Max(maxVal, 
                        price[j] + val[i - j - 1]);
                }

                val[i] = maxVal;
            }

            return val[n];
        }

        public static long FibonacciWithRecursion(int n)
        {
            if(n <= 1) 
            {
                return 1;
            }

            return FibonacciWithRecursion(n - 1) + FibonacciWithRecursion(n - 2);
        }

        public static long FibonacciWithDynamicProgramming(int n)
        {
            var mem = new Dictionary<int, long>();
            return FibonacciWithDynamicProgrammingImpl(n, mem);
        }

        private static long FibonacciWithDynamicProgrammingImpl(int n, IDictionary<int, long> mem)
        {
            if(n <= 1) 
            {
                return 1;
            }

            if(mem.ContainsKey(n))
            {
                return mem[n];
            }

            var res = FibonacciWithDynamicProgrammingImpl(n - 1, mem) + FibonacciWithDynamicProgrammingImpl(n - 2, mem);
            
            mem.Add(n, res);

            return res;
        }
    }
}