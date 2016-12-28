using System;

namespace CodilityTest
{
    public static class Utils
    {
        // see: https://codesays.com/2014/solution-to-perm-check-by-codility/
        // problem: https://codility.com/programmers/lessons/4-counting_elements/perm_check/
        // submission: https://codility.com/demo/results/trainingCZ7D2Z-DDH/

        public static int IsPerm(int[] A)
        {
            if(A == null) throw new ArgumentNullException(nameof(A));
            if(A.Length == 0) throw new ArgumentOutOfRangeException(nameof(A));

            var bag = new int[A.Length];

            for(int i= 0; i< A.Length; i++)
            {
                if (A[i] < 1 || A[i] > A.Length) 
                {
                    // Out of range
                    return 0;
                }
                else if(bag[A[i]-1] == 1) 
                {
                    // met before
                    return 0;
                }
                else 
                {
                    // first time meet
                    bag[A[i]-1] = 1;
                }
            }

            return 1;
        }
    }
}