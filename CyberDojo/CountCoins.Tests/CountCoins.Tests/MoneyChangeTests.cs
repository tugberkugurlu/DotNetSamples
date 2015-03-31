using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CountCoins.Tests
{
    /*
        There are four types of common coins in US currency:
          quarters (25 cents)
          dimes (10 cents)
          nickels (5 cents) 
          pennies (1 cent)
  
        There are 6 ways to make change for 15 cents:
          A dime and a nickel;
          A dime and 5 pennies;
          3 nickels;
          2 nickels and 5 pennies;
          A nickel and 10 pennies;
          15 pennies.
  
        How many ways are there to make change for a dollar
        using these common coins? (1 dollar = 100 cents).
     */

    public struct MoneyType : IEquatable<MoneyType>
    {
        private readonly string _name;
        private readonly int _cents;

        public MoneyType(string name, int cents)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (cents < 1)
            {
                throw new ArgumentOutOfRangeException("cents", cents, "Cents cannot be lower than 1.");
            }

            _name = name;
            _cents = cents;
        }

        public string Name
        {
            get { return _name; }
        }

        public int Cents
        {
            get { return _cents; }
        }

        public static bool operator ==(MoneyType a, MoneyType b)
        {
            return a.Name == b.Name;
        }

        public static bool operator !=(MoneyType a, MoneyType b)
        {
            return !(a == b);
        }

        public bool Equals(MoneyType other)
        {
            return string.Equals(_name, other._name) && _cents == other._cents;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is MoneyType && Equals((MoneyType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_name != null ? _name.GetHashCode() : 0) * 397) ^ _cents;
            }
        }

        public static MoneyType Quarters
        {
            get
            {
                return new MoneyType("quarters", 25);
            }
        }

        public static MoneyType Dimes
        {
            get
            {
                return new MoneyType("dimes", 10);
            }
        }

        public static MoneyType Nickels
        {
            get
            {
                return new MoneyType("nickels", 5);
            }
        }

        public static MoneyType Pennies
        {
            get
            {
                return new MoneyType("pennies", 1);
            }
        }
    }

    public class MoneyChange
    {
    }

    public class MoneyChangeTests
    {
    }

    // https://github.com/mjgpy3/CyberDojoSolutions/blob/master/C%23/CountCoins/CountCoinsTest.cs
    public class CountCoinsTest
    {
        [Fact]
        public void ThereAreSixWaysToMake15Cents()
        {
            const int expected = 6;
            int actual = CountCoins.CountEm(15);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThereIsOneWayToMake1Cent()
        {
            const int expected = 1;
            int actual = CountCoins.CountEm(1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ThereAreTwoWaysToMake5Cents()
        {
            const int expected = 2;
            int actual = CountCoins.CountEm(5);
            Assert.Equal(expected, actual);
        }
    }

    public class CountCoins
    {
        public static int CountEm(int amount)
        {
            // quarters (25 cents)
            // dimes (10 cents)
            // nickels (5 cents) 
            // pennies (1 cent)

            int count = 0;

            for (int nP = 0; nP <= amount; nP++)
            {
                for (int nN = 0; nN <= amount/5.0; nN++)
                {
                    for (int nD = 0; nD <= amount/10.0; nD++)
                    {
                        for (int nQ = 0; nQ <= amount/25.0; nQ++)
                        {
                            if (nP + 5*nN + 10*nD + 25*nQ == amount)
                            {
                                count += 1;
                            }
                        }
                    }
                }
            }

            return count;
        }
    }
}