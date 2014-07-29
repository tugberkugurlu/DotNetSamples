using System;
using System.Globalization;

namespace AHundredDoorsInARow
{
    public class Door : IComparable<Door>
    {
        private readonly int _sequance;

        public Door(int sequance) : this(sequance, DoorState.Closed)
        {
        }

        public Door(int sequance, DoorState state)
        {
            if (sequance < 1)
            {
                throw new ArgumentOutOfRangeException("sequance", sequance.ToString(CultureInfo.InvariantCulture), "Door's sequance cannot be smaller than '0'.");
            }

            _sequance = sequance;
            State = state;
        }

        public int Sequance
        {
            get { return _sequance; }
        }

        public DoorState State { get; private set; }

        public void Toggle()
        {
            State = (State == DoorState.Closed)
                ? DoorState.Open
                : DoorState.Closed;
        }

        public int CompareTo(Door other)
        {
            return Sequance - other.Sequance;
        }
    }
}