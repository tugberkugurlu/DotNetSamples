using System;
using System.Linq;

namespace CodilityTest
{
    public static class Utils
    {
        public static int[] WalkThroghAndReturnOpenDoors(int numberOfDoors)
        {
            if(numberOfDoors < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfDoors));
            }

            var doors = new bool[numberOfDoors];
            for (int step = 1; step <= numberOfDoors; ++step)
            {
                for (int i = step - 1; i < numberOfDoors; i += step)
                {
                    doors[i] = !doors[i];
                }
            }

            return doors.Select((e, i) => new {  Door = i + 1, IsOpen = e })
                .Where(x => x.IsOpen)
                .Select(x => x.Door)
                .ToArray();
        }
    }
}