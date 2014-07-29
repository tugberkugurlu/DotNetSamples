using System;

namespace AHundredDoorsInARow.Console
{
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            var collection = new SequentialDoorsCollection(100);
            collection.Walk();

            foreach (Door door in collection)
            {
                Console.WriteLine("{0}: {1}", door.Sequance, Enum.GetName(typeof (DoorState), door.State));
            }
        }
    }
}
