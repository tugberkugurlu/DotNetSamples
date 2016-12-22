using System;
using System.Text;
using CodilityTest;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var doors = Utils.WalkThroghAndReturnOpenDoors(100);
            foreach (var door in doors)
            {
                System.Console.WriteLine(door);
            }

            System.Console.WriteLine("=============");

            bool[] toggledDoors = SolveDoorExcercise(100);
            String output = FormatOutput(toggledDoors,true);
            Console.Write(output);

            Console.ReadLine();
        }

        private static bool[] SolveDoorExcercise(int numberOfDoors)
        {
            bool[] doors = new bool[numberOfDoors];
            for (int step = 1; step <= numberOfDoors; ++step)
            {
                for (int i = step - 1; i < numberOfDoors; i += step)
                {
                    doors[i] = !doors[i];
                }
            }
            return doors;
        }

        private static string FormatOutput(bool[] doors, bool opened)
        {
            StringBuilder sb = new StringBuilder(doors.Length * 2);
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i] == opened)
                {
                    sb.AppendLine((i + 1).ToString());
                }
            }
            return sb.ToString();
        }
    }
}
