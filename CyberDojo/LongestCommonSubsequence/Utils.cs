using System;
using System.Collections.Generic;
using System.Linq;

namespace CodilityTest
{
    public static class Utils
    {
        /*
            See: http://www.practice.geeksforgeeks.org/problem-page.php?pid=152
            See: https://www.youtube.com/watch?v=NnD96abizww

            Given two sequences, find the length of longest subsequence present in both of them. 
            Both the strings are of uppercase.
        */

        // sample inputs:
        // str1 = abcdaf
        // str2 = acbcf
        // output = 4: new char[] { 'a', 'b', 'c', 'f' }

        public static int CountOfCommonLongestSubsequence(string str1, string str2)
        {
            if(str1 == null) throw new ArgumentNullException(nameof(str1));
            if(str2 == null) throw new ArgumentNullException(nameof(str2));
            if(str1 == string.Empty || str2 == string.Empty) throw new ArgumentException();

            // base is 0 for column and row

            var T = new int[str2.Length + 1, str1.Length + 1];

            for (int i = 0; i < str2.Length; i++)
            {
                for (int j = 0; j < str1.Length; j++)
                {
                    T[i,j] = 0;
                }
            }

            for (int i = 0; i < str2.Length; i++)
            {
                for (int j = 0; j < str1.Length; j++)
                {
                    if(str2[i] == str1[j])
                    {
                        T[i + 1,j + 1] = T[i,j] + 1;
                    }
                    else 
                    {
                        T[i + 1,j + 1] = Math.Max(
                            T[i,j + 1],
                            T[i + 1,j]
                        );
                    }
                }
            }

            return T[str2.Length, str1.Length];
        }
    }
}