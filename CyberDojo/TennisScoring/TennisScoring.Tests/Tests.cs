using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TennisScoring.Tests
{
    /*
        Summary of tennis scoring:

        1. A game is won by the first player to have won at
           least four points in total and at least two points
           more than the opponent.
   
        2. The running score of each game is described in a
           manner peculiar to tennis: scores from zero to three
           points are described as "love", "fifteen", "thirty",
           and "forty" respectively.
   
        3. If at least three points have been scored by each
           player, and the scores are equal, the score is "deuce".
   
        4. If at least three points have been scored by each
           side and a player has one more point than his opponent,
           the score of the game is "advantage" for the player
           in the lead.
     */

    public struct TennisPlayer : IEquatable<TennisPlayer>
    {
        private readonly string _name;

        public TennisPlayer(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public override bool Equals(object obj)
        {
            TennisPlayer player = obj as TennisPlayer;
            return Equals(player);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public bool Equals(TennisPlayer other)
        {
            return other != null &&
                Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator == (TennisPlayer a, TennisPlayer b)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator != (TennisPlayer a, TennisPlayer b)
        {
            return !(a == b);
        }
    }

    public enum ScoreBy
    {
        FirstPlayer = 1,
        SecondPlayer = 2
    }

    public class TennisGame
    {
        private readonly TennisPlayer _fistPlayer;
        private readonly TennisPlayer _secondPlayer;

        public TennisGame(TennisPlayer fistPlayer, TennisPlayer secondPlayer)
        {
            if (fistPlayer == default(TennisPlayer))
            {
                throw new ArgumentNullException("fistPlayer");
            }

            if (secondPlayer == default(TennisPlayer))
            {
                throw new ArgumentNullException("secondPlayer");
            }

            if (fistPlayer == secondPlayer)
            {
                throw new InvalidOperationException("A tennis game needs to be played by two different players.");
            }

            _fistPlayer = fistPlayer;
            _secondPlayer = secondPlayer;
        }

        public void Score(ScoreBy scoreBy)
        {
        }
    }

    public class Tests
    {
        [Fact]
        public void TennisGame_Ctor_Should_Throw_InvalidOperationException_If_Two_Players_Are_The_Same()
        {
            var player1 = new TennisPlayer("foo");
            var player2 = new TennisPlayer("foo");

            Assert.Throws<InvalidOperationException>(() => new TennisGame(player1, player2));
        }
    }
}
