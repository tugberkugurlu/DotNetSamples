using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permutation
{
    class Program
    {
        private readonly static IList<string> Bag = new List<string>();

        static void Main(string[] args)
        {
            const string value = "abcd";
            Console.WriteLine(Factorial(4));

            Console.ReadLine();

            Console.WriteLine("expected: {0}", value.Length * value.Length);
            Permute(value);
            Console.WriteLine("actual: {0}", Bag.Count());

            foreach (var foo in Bag)
            {
                Console.WriteLine(foo);
            }

            Console.ReadLine();
        }

        static int Factorial(int input)
        {
            int answer = 0;

            if (input > 0)
            {
                var count = 1;
                while (count <= input)
                {
                    if (count == 1)
                    {
                        answer = 1;
                        count++;
                    }
                    else
                    {
                        answer = count * answer;
                        count++;
                    }
                }
            }
            else
            {
                Console.WriteLine("Please enter only a positive integer.");
            }

            return answer;
        }

        private static void Permute(string value)
        {
            int length = value.Length;
            bool[] used = new bool[length];
            StringBuilder builder = new StringBuilder(length);
            Permutation(value, length, builder, used, 0);
        }

        private static void Permutation(string value, int length, StringBuilder output, bool[] used, int position)
        {
            if (position == length)
            {
                Bag.Add(output.ToString());
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    // skip already used characters
                    if (used[i])
                    {
                        continue;
                    }

                    // add fixed character to output, and mark it as used
                    output.Append(value[i]);
                    used[i] = true;

                    /* permute over remaining characters starting at position + 1 */
                    Permutation(value, length, output, used, position + 1);

                    /* remove fixed character from output, and unmark it */
                    output.Remove(output.Length - 1, 1);
                    used[i] = false;
                }
            }
        }
    }
}
