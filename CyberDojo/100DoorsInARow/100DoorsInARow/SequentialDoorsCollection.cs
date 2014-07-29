using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace AHundredDoorsInARow
{
    public class SequentialDoorsCollection : IReadOnlyCollection<Door>
    {
        private readonly IReadOnlyCollection<Door> _doors;

        public SequentialDoorsCollection(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count", count.ToString(CultureInfo.InvariantCulture), "Doors count cannot be smaller than '0'.");
            }

            _doors = new ReadOnlyCollection<Door>(Enumerable.Range(1, count).Select(i => new Door(i, DoorState.Closed)).ToList());
        }

        public IEnumerator<Door> GetEnumerator()
        {
            return _doors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _doors.GetEnumerator();
        }

        public int Count
        {
            get { return _doors.Count; }
        }
    }
}