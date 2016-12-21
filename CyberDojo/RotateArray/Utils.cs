using System;

namespace CodilityTest
{
    public class Utils
    {
        // CyclicRotation
        // https://codility.com/demo/results/trainingUMY8UV-C55/#task-0

        public int[] RotateArray(int[] A, int K) 
        {
            if(A == null) throw new ArgumentNullException(nameof(A));
            if(K < 0) throw new ArgumentOutOfRangeException(nameof(K));

            if(A.Length == 0) 
            {
                return A;
            }

            var copyArray = new int[A.Length];
            A.CopyTo(copyArray, 0);

            for (int i = 0; i < K; i++)
            {
                var tempArray = new int[A.Length];
                for (int j = 0; j < A.Length; j++)
                {
                    var indexToPut = j + 1 == A.Length ? 0 : j + 1;
                    tempArray[indexToPut] = copyArray[j];
                }
                
                tempArray.CopyTo(copyArray, 0);
            }

            return copyArray;
        }
    }
}