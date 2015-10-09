using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinationGeneration
{
    class Program
    {
        /// <summary>
        /// refer to: http://stackoverflow.com/questions/10515449/generate-all-combinations-for-a-list-of-strings
        /// </summary>
        static void Main(string[] args)
        {
            IEnumerable<string> allValues = new List<string>() { "A1", "A2", "A3", "B1", "B2", "C1" };
            var combinations = allValues.GenerateCombinations();
            foreach (var item in combinations)
            {
                Console.WriteLine(string.Join(", ", item));
            }

            Console.ReadLine();
        }
    }

    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> GenerateCombinations<T>(this IEnumerable<T> source)
        {
            for (int i = 0; i < (1 << source.Count()); i++)
            {
                yield return ConstructSetFromBits(i).Select(n => source.ElementAt(n));
            }
        }

        private static IEnumerable<int> ConstructSetFromBits(int i)
        {
            for (int n = 0; i != 0; i /= 2, n++)
            {
                if ((i & 1) != 0)
                {
                    yield return n;
                }
            }
        }
    }
}
