namespace FrogJumps
{
    public class Utils
    {
        // X, Y and D are integers within the range [1..1,000,000,000];

        /*
            expected worst-case time complexity is O(1); 
            expected worst-case space complexity is O(1).
        */

        /// <param name="X">Current position of the frog."</param>
        /// <param name="Y">The place which indicates the position greater than or equal to, which frog wants to get to.</param>
        /// <param name="D">The fixed distance that frogs can jump</param>
        public static int MinimumSteps(int X, int Y, int D)
        {
            var distance = Y - X;
            var extraSpace = distance % D;
            var shouldJumpOneMoreTime = extraSpace != 0;
            
            return (distance/D) + (shouldJumpOneMoreTime ? 1 : 0);
        }
    }
}