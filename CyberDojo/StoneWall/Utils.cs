using System;

namespace CodilityTest
{
    public static class Utils
    {
        // This problem is explained in a very fucked up way!
        // see: http://csharplabtests.blogspot.co.uk/2015/06/codility-lesson-5-stonewall.html
        // result, copied from above blog: https://codility.com/demo/results/trainingQ9D6NJ-AF9/

        // N is an integer within the range [1..100,000];
        // each element of array H is an integer within the range [1..1,000,000,000].

        /// <param name="H">Array H of N positive integers specifying the height of the wall.</param>
        /// <returns>The minimum number of blocks needed to build it.</returns>
        public static long MinimumNumberOfBlocks(int[] H)
        {
            if(H == null) throw new ArgumentNullException(nameof(H));
            if(H.Length < 1 || H.Length > 100000) throw new ArgumentOutOfRangeException(nameof(H));

            var lengthOfTheWall = H.Length;
            var stones = 0;
            var wall = new int[lengthOfTheWall];
            var wallNum = 0;

            for (int i = 0; i < lengthOfTheWall; i++)
            {
                while (wallNum > 0 && wall[wallNum -1] > H[i]) wallNum--;
                if(wallNum > 0 && wall[wallNum - 1] == H[i]) 
                {
                    continue;
                }
                else 
                {
                    stones++;
                    wall[wallNum] = H[i];
                    wallNum += 1;
                }
            }

            return stones;
        }
    }
}