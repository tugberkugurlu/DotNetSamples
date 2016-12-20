using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciSequences
{
    class Program
    {
        /*
          Please implement a function which returns the nth number in Fibonacci sequences with an input n.
        */

        /*
            See https://en.wikipedia.org/wiki/Fibonacci_number:
            In mathematics, the Fibonacci numbers are the numbers in the following integer sequence, called the 
            Fibonacci sequence, and characterized by the fact that every number after the first two is the sum 
            of the two preceding ones.
        */

        static void Main(string[] args)
        {
            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine(Fibonacci.Calculate(i));
            }

            Console.ReadLine();
        }
    }

    public static class Fibonacci
    {
        public static int Calculate(int n)
        {
            int a = 0;
            int b = 1;

            for (int i = 0; i < n; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }

            return a;
        }
    }
}
