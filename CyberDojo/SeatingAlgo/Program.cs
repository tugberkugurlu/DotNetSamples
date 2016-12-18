using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class Aircraft 
    {
        public IReadOnlyList<Seat> SeatGroups { get; }
    }

    public class AircraftSeatingGroup
    {
        public IReadOnlyList<Seat> Seats { get; }
    }

    public class Seat
    {
        public Seat(SeatLocation location, SeatArrangement seatArrangement)
        {
            Location = Location;
            Arrangement = seatArrangement;
        }

        public string Name => $"{Location.Row}{Location.Column}";
        public SeatLocation Location { get; }
        public SeatArrangement Arrangement { get; }
    }

    public enum SeatingGroupLayoutType 
    {
        
    }

    public enum SeatArrangement
    {
        Window = 1,
        Aisle = 2,
        Middle = 3
    }

    public struct SeatLocation
    {
        public SeatLocation(int row, char column)
        {
            if (row < 0) throw new ArgumentOutOfRangeException(nameof(row));
            if (!char.IsLetter(column)) throw new ArgumentException(nameof(column));

            Row = row;
            Column = char.ToUpper(column);
        }

        public int Row { get; }
        public char Column { get; set; }
    }
}
