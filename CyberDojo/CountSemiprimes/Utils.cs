using System;
using System.Linq;

namespace CodilityTest
{
    public static class Utils
    {
        // https://codility.com/programmers/lessons/11-sieve_of_eratosthenes/count_semiprimes/

        public static int[] CountOfSemiprimes(int N, int[] P, int[] Q)
        {
            var primes = Sieve(N);

            var semiprimes = new int[N+1];
            var sp = new int[N+1];
            long semiprime = 0;

            for (int i = 0; i < primes.Length; i++)
            {
                for (int j = i; j < primes.Length; j++)
                {
                    semiprime = (long)primes[i] * (long)primes[i];
                    if(semiprime > N)
                    {
                        break;
                    }

                    semiprimes[(int)semiprime] = 1;
                    sp[(int)semiprime] = 1;
                }
            }

            for(int i=1; i<semiprimes.Length; i++) {
                semiprimes[i] += semiprimes[i-1];
            }

            int[] count = new int[Q.Length];

            for(int i=0; i<Q.Length; i++) {
                count[i] = semiprimes[Q[i]] - semiprimes[P[i]] + sp[P[i]];
            }

            return count;
        }

        private static int[] Sieve(int n)
        {
            if(n < 2) throw new ArgumentOutOfRangeException(nameof(n));

            if(n < 3)
            {
                return new[] { 2 };
            }

            var somePrimeNumbers = Enumerable.Range(3, n)
                .Where(x => x % 2 != 0)
                .Where(x => x % 3 != 0)
                .Where(x => x % 5 != 0);

            return new [] { 2, 3, 5 }.Concat(somePrimeNumbers).ToArray();
        }
    }
}