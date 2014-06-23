using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryMath
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> allAvailableNumbers = Enumerable.Range(1, 64).ToList();
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
            return _first.ToString(CultureInfo.InvariantCulture) +
                   _second.ToString(CultureInfo.InvariantCulture) +
                   _third.ToString(CultureInfo.InvariantCulture) +
                   _forth.ToString(CultureInfo.InvariantCulture) +
                   _fifth.ToString(CultureInfo.InvariantCulture) +
                   _sixth.ToString(CultureInfo.InvariantCulture);
        }
    }
}
