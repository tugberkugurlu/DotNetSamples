using System.Collections;
using System.Linq;

namespace AHundredDoorsInARow
{
    public static class SequentialDoorsCollectionExtensions
    {
        public static void Walk(this SequentialDoorsCollection collection)
        {
            for (int i = 1; i <= collection.Count; i++)
            {
                IEnumerable walkableDoors = collection
                    .Where(door => (door.Sequance % i) == 0)
                    .ToList();

                foreach (Door walkableDoor in walkableDoors)
                {
                    walkableDoor.Toggle();
                }
            }
        }
    }
}