using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LotteryMath
{
    class Program
    {
        // Read: Generating permutations of a set (most efficiently) - http://stackoverflow.com/questions/11208446

        private readonly static ConcurrentBag<LotteryCoupon> Bag = new ConcurrentBag<LotteryCoupon>();
        private readonly static IEnumerable<int> AllAvailableNumbers = Enumerable.Range(1, 64).ToList();

        static void Main(string[] args)
        {
            LotteryCoupon coupon = GenerateLotteryCoupon();
            Console.WriteLine(coupon);
            Console.ReadLine();
        }

        static LotteryCoupon GenerateLotteryCoupon()
        {
            List<int> numbers = new List<int>();
            while (numbers.Count != 6)
            {
                AddNewNumber(numbers);
            }

            return new LotteryCoupon(
                numbers.First(),
                numbers.Skip(1).First(),
                numbers.Skip(2).First(),
                numbers.Skip(3).First(),
                numbers.Skip(4).First(),
                numbers.Skip(5).First());
        }

        static void AddNewNumber(List<int> numbers)
        {
            int num = new Random().Next(1, 64);
            if (numbers.Any(x => x == num) == false)
            {
                numbers.Add(num);
            }
        }

        private static bool NextPermutation(int[] numList)
        {
            /*
             Taken from http://stackoverflow.com/a/11208543/463785
             
             Knuths
             1. Find the largest index j such that a[j] < a[j + 1]. If no such index exists, the permutation is the last permutation.
             2. Find the largest index l such that a[j] < a[l]. Since j + 1 is such an index, l is well defined and satisfies j < l.
             3. Swap a[j] with a[l].
             4. Reverse the sequence from a[j + 1] up to and including the final element a[n].

             */

            int largestIndex = -1;
            for (var i = numList.Length - 2; i >= 0; i--)
            {
                if (numList[i] < numList[i + 1])
                {
                    largestIndex = i;
                    break;
                }
            }

            if (largestIndex < 0)
            {
                return false;
            }

            int largestIndex2 = -1;
            for (int i = numList.Length - 1; i >= 0; i--)
            {
                if (numList[largestIndex] < numList[i])
                {
                    largestIndex2 = i;
                    break;
                }
            }

            int tmp = numList[largestIndex];
            numList[largestIndex] = numList[largestIndex2];
            numList[largestIndex2] = tmp;

            for (int i = largestIndex + 1, j = numList.Length - 1; i < j; i++, j--)
            {
                tmp = numList[i];
                numList[i] = numList[j];
                numList[j] = tmp;
            }

            return true;
        }
    }

    public class LotteryCouponComparer : IEqualityComparer<LotteryCoupon>
    {
        public bool Equals(LotteryCoupon x, LotteryCoupon y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(LotteryCoupon obj)
        {
            return obj.GetHashCode();
        }
    }

    public class LotteryCoupon
    {
        private readonly int _first;
        private readonly int _second;
        private readonly int _third;
        private readonly int _forth;
        private readonly int _fifth;
        private readonly int _sixth;

        public LotteryCoupon(int first, int second, int third, int forth, int fifth, int sixth)
        {
            if (first < 1 && first > 64) throw new IndexOutOfRangeException();
            if (second < 1 && second > 64) throw new IndexOutOfRangeException();
            if (third < 1 && third > 64) throw new IndexOutOfRangeException();
            if (forth < 1 && forth > 64) throw new IndexOutOfRangeException();
            if (fifth < 1 && fifth > 64) throw new IndexOutOfRangeException();
            if (sixth < 1 && sixth > 64) throw new IndexOutOfRangeException();

            _first = first;
            _second = second;
            _third = third;
            _forth = forth;
            _fifth = fifth;
            _sixth = sixth;
        }

        public int First
        {
            get { return _first; }
        }

        public int Second
        {
            get { return _second; }
        }

        public int Third
        {
            get { return _third; }
        }

        public int Forth
        {
            get { return _forth; }
        }

        public int Fifth
        {
            get { return _fifth; }
        }

        public int Sixth
        {
            get { return _sixth; }
        }

        public override int GetHashCode()
        {
            return int.Parse(ToString());
        }

        public override string ToString()
        {
            // TODO: This is incorrect. Fix this later...

            return _first.ToString(CultureInfo.InvariantCulture) +
                   _second.ToString(CultureInfo.InvariantCulture) +
                   _third.ToString(CultureInfo.InvariantCulture) +
                   _forth.ToString(CultureInfo.InvariantCulture) +
                   _fifth.ToString(CultureInfo.InvariantCulture) +
                   _sixth.ToString(CultureInfo.InvariantCulture);
        }
    }
}
